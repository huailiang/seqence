using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public abstract class EditorObject
    {
        public abstract void OnInit(XTimelineObject obj);

        public static implicit operator bool(EditorObject obj)
        {
            return obj != null;
        }
    }
}
