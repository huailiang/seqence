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
            st = 0;
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
                        OnDrag(p);
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

            tmp.y = rect.y + tmp.height / 3;
            EditorGUI.LabelField(tmp, clip.Display);
        }

        private bool draging;
        private float st;

        private void OnMouseDown(Vector2 v2)
        {
            draging = true;
            st = v2.x;
        }

        private void OnDrag(Vector2 v2)
        {
            draging = true;
            float delta = st - v2.x;
            // clip.start += delta;
        }

        private void OnMouseUp(Vector2 v2)
        {
            draging = false;
        }
    }
}
