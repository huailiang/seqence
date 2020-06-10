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
            EditorGUI.DrawRect(tmp, Color.gray);
            EditorGUI.DrawOutline(tmp, 1, Color.white);

            var e = Event.current;
            Vector2 p = e.mousePosition;
            if (tmp.Contains(p))
            {
                switch (e.type)
                {
                    case EventType.MouseDrag:
                    case EventType.ScrollWheel:
                        OnDrag(e);
                        break;
                }
            }

            tmp.y = rect.y;
            EditorGUI.LabelField(tmp, clip.Display, TimelineStyles.fontClip);
        }


        private void OnDrag(Event e)
        {
            rectX += e.delta.x;
            clip.start = TimelineWindow.inst.PiexlToTime(rectX);
            e.Use();
        }
    }
}
