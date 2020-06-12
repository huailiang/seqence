using UnityEngine;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    public class EditorPostprocessTrack : EditorTrack
    {
        protected override Color trackColor
        {
            get { return Color.magenta; }
        }


        protected override TrackData BuildChildData(int i)
        {
            throw new System.NotImplementedException();
        }
    }
}
