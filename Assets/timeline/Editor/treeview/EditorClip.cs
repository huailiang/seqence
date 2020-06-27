using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public enum DragMode { None, Drag, Left, Right }
    public struct EditorClip
    {
        public EditorTrack track;
        public IClip clip;
        public Rect rect;
        public DragMode mode;
        Event e;
        
        public EditorClip(EditorTrack tr, IClip c)
        {
            this.track = tr;
            this.clip = c;
            rect = Rect.zero;
            mode = DragMode.None;
            e = Event.current;
        }

        public void OnGUI()
        {
            rect = track.RenderRect;
            rect.x = TimelineWindow.inst.TimeToPixel(clip.start);
            float y = TimelineWindow.inst.TimeToPixel(clip.end);
            var timeRect = TimelineWindow.inst.timeAreaRect;
            rect.x = Mathf.Max(rect.x, timeRect.x);
            y = Mathf.Min(y, timeRect.xMax);
            rect.width = y - rect.x;
            rect.height = rect.height - 2;
            if (rect.width < 0) rect.width = 0;
            EditorGUI.DrawRect(rect, Color.gray);
            EditorGUI.DrawOutline(rect, 1, Color.white);

            Rect left = rect;
            left.x = rect.x - Mathf.Min(10, rect.width / 4);
            left.x = Mathf.Max(left.x, timeRect.x);
            left.width = Mathf.Min(20, rect.width / 2);
            EditorGUIUtility.AddCursorRect(left, MouseCursor.SplitResizeLeftRight);
            Rect right = left;
            right.x = rect.x + rect.width - Mathf.Min(10, rect.width / 4);
            right.x = Mathf.Max(right.x, timeRect.x);
            EditorGUIUtility.AddCursorRect(right, MouseCursor.SplitResizeLeftRight);

            Vector2 p = e.mousePosition;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (left.Contains(p))
                    {
                        mode = DragMode.Left;
                    }
                    else if (right.Contains(p))
                    {
                        mode = DragMode.Right;
                    }
                    else if (rect.Contains(e.mousePosition))
                    {
                        mode = DragMode.Drag;
                    }
                    else
                    {
                        mode = DragMode.None;
                    }
                    break;
                case EventType.MouseUp:
                    if (mode != DragMode.None)
                    {
                        track?.track?.SortClip();
                    }
                    mode = DragMode.None;
                    break;
                case EventType.MouseDrag:
                case EventType.ScrollWheel:
                    Drag(e);
                    break;
            }
            EditorGUI.LabelField(rect, clip.Display, TimelineStyles.fontClip);
            MixProcessor();
        }

        private void Drag(Event e)
        {
            if (mode == DragMode.Left)
            {
                DragStart(e);
            }
            else if (mode == DragMode.Right)
            {
                DragEnd(e);
            }
            else if (mode == DragMode.Drag)
            {
                OnDrag(e);
            }
        }

        private void MixProcessor()
        {
            var clips = track.eClips;
            foreach (var c in clips)
            {
                if (c.clip != this.clip)
                {
                    if (IsInRange(c.clip, clip.start))
                    {
                        var r = rect;
                        r.width = c.rect.x + c.rect.width - rect.x;
                        ProcesMixIn(r);
                    }
                    if (IsInRange(c.clip, clip.end))
                    {
                        var r = rect;
                        r.x = c.rect.x;
                        r.width = rect.x + rect.width - r.x;
                        ProcesMixOut(r);
                    }
                }
            }
        }

        private bool IsInRange(IClip clip, float t)
        {
            if (clip != null)
            {
                return t < clip.end && t > clip.start;
            }
            return false;
        }

        private bool ValidRange(Rect r)
        {
            var timeRect = TimelineWindow.inst.timeAreaRect;
            return r.x >= timeRect.x && r.xMax <= timeRect.xMax;
        }

        private void ProcesMixIn(Rect mixInRect)
        {
            if (ValidRange(mixInRect) && mixInRect.width > 0)
            {
                var clipStyle = TimelineStyles.timelineClip;
                var texture = clipStyle.normal.background;
                ClipRenderer.RenderTexture(mixInRect, texture, TimelineStyles.blendMixIn.normal.background,
                    Color.black);

                Graphics.DrawLineAA(2.5f, new Vector3(mixInRect.xMin, mixInRect.yMax - 1f, 0),
                    new Vector3(mixInRect.xMax, mixInRect.yMin + 1f, 0), Color.white);
            }
        }

        private void ProcesMixOut(Rect mixOutRect)
        {
            if (ValidRange(mixOutRect) && mixOutRect.width > 0)
            {
                var clipStyle = TimelineStyles.timelineClip;
                var texture = clipStyle.normal.background;
                ClipRenderer.RenderTexture(mixOutRect, texture, TimelineStyles.blendMixOut.normal.background,
                    Color.black);

                Graphics.DrawLineAA(2.5f, new Vector3(mixOutRect.xMin, mixOutRect.yMax - 1f, 0),
                    new Vector3(mixOutRect.xMax, mixOutRect.yMin + 1f, 0), Color.white);
            }
        }

        private void DragStart(Event e)
        {
            rect.x = TimelineWindow.inst.TimeToPixel(clip.start);
            rect.x += e.delta.x;
            var start2 = TimelineWindow.inst.PiexlToTime(rect.x);
            if (start2 >= 0 && start2 <= clip.end)
            {
                clip.duration -= (start2 - clip.start);
                clip.start = start2;
                e.Use();
            }
        }

        private void DragEnd(Event e)
        {
            rect.x = TimelineWindow.inst.TimeToPixel(clip.end);
            rect.x += e.delta.x;
            var end = TimelineWindow.inst.PiexlToTime(rect.x);
            if (end > clip.start)
            {
                clip.duration += (end - clip.end);
                e.Use();
            }
        }


        private void OnDrag(Event e)
        {
            rect.x += e.delta.x;
            clip.start = TimelineWindow.inst.PiexlToTime(rect.x);
            e.Use();
        }
    }
}
