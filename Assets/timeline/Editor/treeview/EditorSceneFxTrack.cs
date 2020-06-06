using UnityEngine;

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
    }
}
