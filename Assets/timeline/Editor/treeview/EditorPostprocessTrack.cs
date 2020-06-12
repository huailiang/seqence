using UnityEngine;

namespace UnityEditor.Timeline
{
    public class EditorPostprocessTrack : EditorTrack
    {
        protected override Color trackColor
        {
            get { return Color.magenta; }
        }
    }
}
