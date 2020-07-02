using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    [TimelineEditor(typeof(XBoneFxTrack))]
    public class EditorBoneTrack : EditorTrack
    {
        protected override Color trackColor
        {
            get { return Color.green; }
        }

        protected override string trackHeader
        {
            get { return "骨骼特效" + ID; }
        }

        protected override bool warn
        {
            get
            {
                var clips = track.data.clips;
                if (clips != null)
                {
                    foreach (var clip in clips)
                    {
                        var data = clip as BoneFxClipData;
                        if (data == null ||
                            string.IsNullOrEmpty(data.prefab) ||
                            string.IsNullOrEmpty(data.bone))
                            return true;
                    }
                    return false;
                }
                else
                    return false;
            }
        }

        protected override void OnAddClip(float t)
        {
            ObjectSelector.get.Show(null, typeof(GameObject), null, false, null, obj =>
            {
                if (obj != null)
                {
                    AddClip(obj, t);
                }
            }, null);
        }

        protected override void OnDragDrop(UnityEngine.Object[] objs)
        {
            var selectedObjects = from go in objs
                                  where go as GameObject != null
                                  select go as GameObject;
            if (selectedObjects.Count() > 0)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (e.type == EventType.DragPerform)
                {
                    var obj = selectedObjects.First();
                    float t = TimelineWindow.inst.PiexlToTime(e.mousePosition.x);
                    AddClip(obj, t);
                    DragAndDrop.AcceptDrag();
                    e.Use();
                }
            }
        }


        private void AddClip(Object obj, float t)
        {
            BoneFxClipData data = new BoneFxClipData();
            data.start = t;
            data.duration = 10;
            data.prefab = AssetDatabase.GetAssetPath(obj);
            data.seed = 0;
            data.scale = Vector3.one;
            XBoneFxClip clip = new XBoneFxClip((XBoneFxTrack)track, data);
            clip.SetFx((GameObject)obj);
            track.AddClip(clip, data);
        }

        private Object bone, prefab;

        protected override void OnInspectorClip(IClip c)
        {
            base.OnInspectorClip(c);
            XBoneFxClip xc = c as XBoneFxClip;
            var data = c.data as BoneFxClipData;
            prefab = EditorGUILayout.ObjectField("prefab", xc.fx, typeof(GameObject), false);
            if (prefab)
            {
                xc.fx = (GameObject)prefab;
            }
            else if (!string.IsNullOrEmpty(data.prefab))
            {
                xc.fx = AssetDatabase.LoadAssetAtPath<GameObject>(data.prefab);
            }
            else
            {
                EditorGUILayout.HelpBox("fx prefab is null", MessageType.Warning);
            }
            if (!string.IsNullOrEmpty(data.prefab))
            {
                EditorGUILayout.LabelField("fx: " + data.prefab);
            }
            bone = EditorGUILayout.ObjectField("bone", bone, typeof(GameObject), true);
            if (bone)
            {
                GameObject g = bone as GameObject;
                data.bone = GetHieracyPath(g.transform);
            }
            else if (!string.IsNullOrEmpty(data.bone))
            {
                var t = base.track.root;
                if (t is XBindTrack bt)
                {
                    if (bt.bindObj)
                    {
                        Transform tmp = bt.bindObj.transform.Find(data.bone);
                        if (tmp)
                        {
                            bone = tmp.gameObject;
                            xc.fx.transform.parent = tmp;
                        }
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("bind bone is null", MessageType.Warning);
            }
            if (!string.IsNullOrEmpty(data.bone)) EditorGUILayout.LabelField("bone: " + data.bone);

            data.pos = EditorGUILayout.Vector3Field("pos", data.pos);
            data.rot = EditorGUILayout.Vector3Field("rot", data.rot);
            data.scale = EditorGUILayout.Vector3Field("scale", data.scale);
            if (xc.fx != null && bone != null)
            {
                var tf = xc.fx.transform;
                tf.localPosition = data.pos;
                tf.localEulerAngles = data.rot;
                tf.localScale = data.scale;
            }
        }

        protected override void OnSelect()
        {
            base.OnSelect();
            var bind = (track.parent as XBindTrack).bindObj;
            if (bind)
            {
                Selection.activeGameObject = bind.gameObject;
            }
        }

        private string GetHieracyPath(Transform b)
        {
            string p = string.Empty;
            var bind = (track.parent as XBindTrack).bindObj;
            while (b.parent != null && b.name != bind.name)
            {
                p = string.IsNullOrEmpty(p) ? b.name : b.name + "/" + p;
                b = b.parent;
            }
            return p;
        }
    }
}