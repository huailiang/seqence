using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    public class EditorSceneFxTrack : EditorTrack
    {
        protected override Color trackColor
        {
            get { return Color.cyan; }
        }

        protected override void OnGUIHeader()
        {
        }


        protected override void AddClip(object mpos)
        {
            ObjectSelector.get.Show(null, typeof(GameObject), null, false, null, obj =>
            {
                if (obj != null)
                {
                    Vector2 v2 = (Vector2) mpos;
                    float start = TimelineWindow.inst.PiexlToTime(v2.x);
                    SceneFxClipData data = new SceneFxClipData();
                    data.start = start;
                    data.duration = 20;
                    data.prefab = AssetDatabase.GetAssetPath(obj);
                    data.seed = 0;
                    XSceneFxClip clip = new XSceneFxClip((XSceneFxTrack) track, data);
                    clip.SetReference((GameObject) obj);
                    track.AddClip(clip);
                    base.AddClip(mpos);
                }
            }, null);
        }


        protected override TrackData BuildChildData(int i)
        {
            throw new System.NotImplementedException();
        }


        protected override void OnInspectorClip(IClip c)
        {
            XSceneFxClip clip = (XSceneFxClip) c;
            if (clip)
            {
                EditorGUILayout.LabelField(clip.prefabGameObject.name);
            }
        }
    }
}
