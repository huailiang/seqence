using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    [TimelineEditor(typeof(XSceneFxTrack))]
    public class EditorSceneFxTrack : EditorTrack
    {
        protected override Color trackColor
        {
            get { return Color.cyan; }
        }

        protected override string trackHeader
        {
            get { return "场景特效" + ID; }
        }

        protected override void OnGUIHeader()
        {
        }


        protected override void OnAddClip(float t)
        {
            ObjectSelector.get.Show(null, typeof(GameObject), null, false, null, obj =>
            {
                if (obj != null)
                {
                    SceneFxClipData data = new SceneFxClipData();
                    data.start = t;
                    data.duration = 20;
                    data.prefab = AssetDatabase.GetAssetPath(obj);
                    data.seed = 0;
                    XSceneFxClip clip = new XSceneFxClip((XSceneFxTrack) track, data);
                    clip.SetReference((GameObject) obj);
                    track.AddClip(clip, data);
                }
            }, null);
        }

        protected override void OnSelect()
        {
            base.OnSelect();
            if (track.clips != null)
            {
                Selection.activeGameObject = null;
                foreach (var clip in track.clips)
                {
                    XSceneFxClip fxClip = clip as XSceneFxClip;
                    if (fxClip)
                    {
                        Selection.Add(fxClip.prefabGameObject);
                    }
                }
            }
        }

        protected override void OnInspectorClip(IClip c)
        {
            base.OnInspectorClip(c);
            XSceneFxClip clip = (XSceneFxClip) c;
            if (clip)
            {
                EditorGUILayout.LabelField(clip.prefabGameObject.name);
                EditorGUILayout.ObjectField("fx", clip.prefabGameObject, typeof(GameObject), true);
                var go = clip.prefabGameObject;
                if (go)
                {
                    var tf = go.transform;
                    tf.localPosition = EditorGUILayout.Vector3Field("pos", tf.localPosition);
                    tf.localEulerAngles = EditorGUILayout.Vector3Field("rot", tf.localEulerAngles);
                    tf.localScale = EditorGUILayout.Vector3Field("scale", tf.localScale);
                }
            }
        }
    }
}
