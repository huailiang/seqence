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

            Rect left = tmp;
            left.x = tmp.x - tmp.width / 4;
            left.width = tmp.width / 2;
            EditorGUIUtility.AddCursorRect(left, MouseCursor.SplitResizeLeftRight);
            Rect right = left;
            right.x = tmp.x + tmp.width * 0.75f;
            EditorGUIUtility.AddCursorRect(right, MouseCursor.SplitResizeLeftRight);

            var e = Event.current;
            Vector2 p = e.mousePosition;

            switch (e.type)
            {
                case EventType.MouseDrag:
                case EventType.ScrollWheel:
                    if (left.Contains(p))
                    {
                        DragStart(e);
                    }
                    else if (right.Contains(p))
                    {
                        DragEnd(e);
                    }
                    else if (tmp.Contains(p))
                    {
                        OnDrag(e);
                    }
                    break;
            }
            tmp.y = rect.y;
            EditorGUI.LabelField(tmp, clip.Display, TimelineStyles.fontClip);
        }

        private void DragStart(Event e)
        {
            rectX += e.delta.x;
            var start2 = TimelineWindow.inst.PiexlToTime(rectX);
            clip.duration += (start2 - clip.start);
            clip.start = start2;
            e.Use();
        }

        private void DragEnd(Event e)
        {
            rectX = TimelineWindow.inst.TimeToPixel(clip.end);
            rectX += e.delta.x;
            var end = TimelineWindow.inst.TimeToPixel(rectX);
            clip.duration = end - clip.start;
            e.Use();
        }


        private void OnDrag(Event e)
        {
            rectX += e.delta.x;
            clip.start = TimelineWindow.inst.PiexlToTime(rectX);
            e.Use();
        }
    }
}
