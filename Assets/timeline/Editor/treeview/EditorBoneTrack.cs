using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public class EditorBoneTrack : EditorTrack
    {
        protected override Color trackColor
        {
            get { return Color.green; }
        }
    }
}
