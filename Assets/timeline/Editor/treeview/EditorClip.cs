using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public struct EditorClip
    {
        public EditorTrack track;
        public IClip clip;

        private float rectX;

        public EditorClip(EditorTrack tr, IClip c)
        {
            this.track = tr;
            this.clip = c;
            rectX = 0;
        }

        public void OnGUI()
        {
            var rect = track.rect;
            Rect tmp = rect;
            rectX = TimelineWindow.inst.TimeToPixel(clip.start);
            float y = TimelineWindow.inst.TimeToPixel(clip.end);
            tmp.x = rectX;
            tmp.width = y - rectX;
            tmp.height = rect.height - 2;
            EditorGUI.DrawRect(tmp, Color.white);
            tmp.x = rectX + 1;
            tmp.width = y - rectX - 2;
            tmp.y = rect.y + 1;
            tmp.height = rect.height - 3;
            EditorGUI.DrawRect(tmp, Color.gray);

            var e = Event.current;
            Vector2 p = e.mousePosition;
            if (rect.Contains(p))
            {
                switch (e.type)
                {
                    case EventType.MouseUp:
                        OnMouseUp(p);
                        e.Use();
                        break;
                    case EventType.MouseDrag:
                    case EventType.ScrollWheel:
                        OnDrag(e);
                        e.Use();
                        break;
                    case EventType.MouseDown:
                        OnMouseDown(p);
                        e.Use();
                        break;
                }
            }

            tmp.y = rect.y;
            EditorGUI.LabelField(tmp, clip.Display, TimelineStyles.fontClip);
        }


        private void OnMouseDown(Vector2 v2)
        {
            Debug.Log("mouse down");
        }

        private void OnDrag(Event e)
        {
            rectX += e.delta.x;
            clip.start = TimelineWindow.inst.PiexlToTime(rectX);
        }

        private void OnMouseUp(Vector2 v2)
        {
        }
    }
}
