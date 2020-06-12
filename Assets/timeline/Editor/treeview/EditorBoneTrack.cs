using UnityEngine;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    public class EditorBoneTrack : EditorTrack
    {
        protected override Color trackColor
        {
            get { return Color.green; }
        }
        

        protected override TrackData BuildChildData(int i)
        {
            throw new System.NotImplementedException();
        }
    }
}
