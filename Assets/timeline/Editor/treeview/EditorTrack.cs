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

        protected bool triger
        {
            get
            {
                var pos = Event.current.mousePosition;
                return head.Contains(pos) || rect.Contains(pos);
            }
        }

        public void OnGUI()
        {
            if (pm == null)
            {
                pm = new GenericMenu();
            }
            var backgroundColor = select
                ? TimelineStyles.colorDuration
                : TimelineStyles.markerHeaderDrawerBackgroundColor;
            EditorGUI.DrawRect(rect, backgroundColor);
            var headColor = backgroundColor;
            EditorGUI.DrawRect(head, headColor);

            GUIHeader();
            GUIContent();

            var e = Event.current;
            if (e.type == EventType.ContextClick)
            {
                if (triger)
                {
                    pm.AddDisabledItem(EditorGUIUtility.TrTextContent("Insert Before"));
                    pm.AddSeparator("");
                    pm.AddItem(EditorGUIUtility.TrTextContent("Add Clip \t"), false, AddClip);
                    pm.AddItem(EditorGUIUtility.TrTextContent("Delete \t"), false, DeleteClip);
                    if (track.mute)
                    {
                        pm.AddItem(EditorGUIUtility.TrTextContent("UnMute Clip \t"), false, UnmuteClip);
                    }
                    else
                    {
                        pm.AddItem(EditorGUIUtility.TrTextContent("Mute Clip \t"), false, MuteClip);
                    }
                    pm.ShowAsContext();
                }
            }
            else if (e.type == EventType.MouseDown)
            {
                if (triger)
                {
                    @select = !@select;
                    TimelineWindow.inst.Repaint();
                }
            }
        }

        protected void GUIHeader()
        {
            GUILayout.BeginArea(head);
            GUILayout.BeginHorizontal();
            GUILayout.Label(track.ToString());
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        protected virtual void OnGUIHeader()
        {
        }

        protected void GUIContent()
        {
            var clips = track.clips;
            Rect tmp = rect;
            if (clips != null)
            {
                for (int i = 0; i < clips.Length; i++)
                {
                    var clip = clips[i];
                    float x = TimelineWindow.inst.TimeToPixel(clip.start);
                    float y = TimelineWindow.inst.TimeToPixel(clip.end);
                    tmp.x = x;
                    tmp.width = y - x;
                    EditorGUI.DrawRect(tmp, TimelineStyles.colorRecordingClipOutline);
                }
            }
            GUILayout.BeginArea(rect);
            OnGUIContent();
            GUILayout.EndArea();
        }

        protected virtual void OnGUIContent()
        {
        }

        private void AddClip()
        {
            Debug.Log("Add Clip");
        }

        private void DeleteClip()
        {
            Debug.Log("delete Click");
            TimelineWindow.inst.Repaint();
        }

        private void MuteClip()
        {
            track.SetMute(true);
            TimelineWindow.inst.Repaint();
        }

        private void UnmuteClip()
        {
            track.SetMute(false);
            TimelineWindow.inst.Repaint();
        }
    }
}
