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
            get { return bone == null || prefab == null; }
        }

        protected override void OnAddClip(float t)
        {
            ObjectSelector.get.Show(null, typeof(GameObject), null, false, null, obj =>
            {
                if (obj != null)
                {
                    BoneFxClipData data = new BoneFxClipData();
                    data.start = t;
                    data.duration = 20;
                    data.prefab = AssetDatabase.GetAssetPath(obj);
                    data.seed = 0;
                    XBoneFxClip clip = new XBoneFxClip((XBoneFxTrack) track, data);
                    clip.SetFx((GameObject) obj);
                    track.AddClip(clip, data);
                }
            }, null);
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
                xc.fx = (GameObject) prefab;
            }
            else if (!string.IsNullOrEmpty(data.prefab))
            {
                xc.fx = AssetDatabase.LoadAssetAtPath<GameObject>(data.prefab);
            }
            if(prefab==null)
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
                        if (tmp) bone = tmp.gameObject;
                    }
                }
            }
            if (bone == null)
            {
                EditorGUILayout.HelpBox("bind bone is null", MessageType.Warning);
            }
            if (!string.IsNullOrEmpty(data.bone)) EditorGUILayout.LabelField("bone: " + data.bone);
        }

        protected override void OnSelect()
        {
            base.OnSelect();
            if (track.clips != null)
            {
                Selection.activeGameObject = null;
                foreach (var clip in track.clips)
                {
                    if (clip is XBoneFxClip xc)
                    {
                        Selection.Add(xc.fx);
                    }
                }
            }
        }

        private string GetHieracyPath(Transform b)
        {
            string p = string.Empty;
            if (b.parent != null)
            {
                p = string.IsNullOrEmpty(p) ? b.name : b.name + "/" + p;
                b = b.parent;
            }
            return p;
        }
    }
}
