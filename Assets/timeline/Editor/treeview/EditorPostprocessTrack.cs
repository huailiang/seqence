using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    public class EditorPostprocessTrack : EditorTrack
    {
        protected override Color trackColor
        {
            get { return Color.magenta; }
        }


        protected override void OnAddClip(float t)
        {
            PostprocessData data = new PostprocessData();
            data.start = t;
            data.duration = 20;
            XPostprocessClip clip = new XPostprocessClip((XPostprocessTrack) track, data);
        }
    }
}
