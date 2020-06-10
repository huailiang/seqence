using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public struct EditorClip
    {
        public EditorTrack track;
        public IClip clip;

        public EditorClip(EditorTrack tr, IClip c)
        {
            this.track = tr;
            this.clip = c;
            draging = false;
        }

        public void OnGUI()
        {
            var rect = track.rect;
            Rect tmp = rect;
            float x = TimelineWindow.inst.TimeToPixel(clip.start);
            float y = TimelineWindow.inst.TimeToPixel(clip.end);
            tmp.x = x;
            tmp.width = y - x;
            tmp.height = rect.height - 2;
            EditorGUI.DrawRect(tmp, Color.white);
            tmp.x = x + 1;
            tmp.width = y - x - 2;
            tmp.y = rect.y + 1;
            tmp.height = rect.height - 3;
            EditorGUI.DrawRect(tmp, Color.gray);

            var e = Event.current;
            Vector2 p = e.mousePosition;
            if (tmp.Contains(p))
            {
                switch (e.type)
                {
                    case EventType.MouseUp:
                        OnMouseUp(p);
                        break;
                    case EventType.MouseDrag:
                    case EventType.ScrollWheel:
                        OnDrag(e);
                        break;
                    case EventType.MouseDown:
                        OnMouseDown(p);
                        break;
                }
            }
            else if (draging)
            {
                OnMouseUp(p);
            }

            tmp.y = rect.y ;
            EditorGUI.LabelField(tmp, clip.Display, TimelineStyles.fontClip);
        }

        private bool draging;

        private void OnMouseDown(Vector2 v2)
        {
            draging = true;
            Debug.Log("mouse down");
        }

        private void OnDrag(Event e)
        {
            draging = true;
            // Debug.Log(e.mousePosition);
            clip.start += e.delta.x;
        }

        private void OnMouseUp(Vector2 v2)
        {
            draging = false;
        }
    }
}
