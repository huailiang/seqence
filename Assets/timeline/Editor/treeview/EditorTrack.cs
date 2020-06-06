using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public class EditorTrack
    {
        public XTrack track;
        public Rect rect;
        public Rect head;
        public bool select;

        private GenericMenu pm;

        public uint ID
        {
            get { return track.ID; }
        }

        protected virtual Color trackColor
        {
            get { return TimelineStyles.colorControl; }
        }

        public void OnGUI()
        {
            if (pm == null)
            {
                pm = new GenericMenu();
            }
            var backgroundColor = select
                ? TimelineStyles.colorTrackSubSequenceBackgroundSelected
                : TimelineStyles.markerHeaderDrawerBackgroundColor;
            EditorGUI.DrawRect(rect, backgroundColor);

            var headColor = backgroundColor;
            EditorGUI.DrawRect(head, headColor);

            GUIHeader();
            GUIContent();

            var e = Event.current;
            if (e.type == EventType.MouseUp)
            {
                if (rect.Contains(e.mousePosition))
                {
                    pm.AddDisabledItem(EditorGUIUtility.TrTextContent("Insert Before"));
                    pm.AddSeparator("");
                    pm.AddItem(EditorGUIUtility.TrTextContent("Add Clip \t"), false, AddClip);
                    pm.AddItem(EditorGUIUtility.TrTextContent("Delete \t"), false, DeleteClip);
                    pm.ShowAsContext();
                }
            }
        }

        protected virtual void GUIHeader()
        {
        }

        protected virtual void GUIContent()
        {
        }

        private void AddClip()
        {
            Debug.Log("Add Clip");
        }

        private void DeleteClip()
        {
            Debug.Log("delete Click");
        }
    }
}
