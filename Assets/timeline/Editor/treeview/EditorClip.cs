using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public struct EditorClip
    {
        public EditorTrack track;
        public IClip clip;
        public Rect rect;

        public EditorClip(EditorTrack tr, IClip c)
        {
            this.track = tr;
            this.clip = c;
            rect = Rect.zero;
        }

        public void OnGUI()
        {
            rect = track.rect;
            rect.x = TimelineWindow.inst.TimeToPixel(clip.start);
            float y = TimelineWindow.inst.TimeToPixel(clip.end);
            rect.width = y - rect.x;
            rect.height = rect.height - 2;
            EditorGUI.DrawRect(rect, Color.gray);
            EditorGUI.DrawOutline(rect, 1, Color.white);

            Rect left = rect;
            left.x = rect.x - Mathf.Min(10, rect.width / 4);
            left.width = Mathf.Min(20, rect.width / 2);
            EditorGUIUtility.AddCursorRect(left, MouseCursor.SplitResizeLeftRight);
            Rect right = left;
            right.x = rect.x + rect.width - Mathf.Min(10, rect.width / 4);
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
                    else if (rect.Contains(p))
                    {
                        OnDrag(e);
                    }
                    break;
            }
            EditorGUI.LabelField(rect, clip.Display, TimelineStyles.fontClip);
            MixProcessor();
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
                    if (IsInRange(c.clip,clip.end))
                    {
                        var r = rect;
                        r.x = c.rect.x ;
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

        private void ProcesMixIn(Rect mixInRect)
        {
            var clipStyle = TimelineStyles.timelineClip;
            var texture = clipStyle.normal.background;
            ClipRenderer.RenderTexture(mixInRect, texture, TimelineStyles.blendMixIn.normal.background, Color.black);

            Graphics.DrawLineAA(2.5f, new Vector3(mixInRect.xMin, mixInRect.yMax - 1f, 0),
                new Vector3(mixInRect.xMax, mixInRect.yMin + 1f, 0), Color.white);
        }


        private void ProcesMixOut(Rect mixOutRect)
        {
            var clipStyle = TimelineStyles.timelineClip;
            var texture = clipStyle.normal.background;
            ClipRenderer.RenderTexture(mixOutRect, texture, TimelineStyles.blendMixOut.normal.background, Color.black);

            Graphics.DrawLineAA(2.5f, new Vector3(mixOutRect.xMin, mixOutRect.yMax - 1f, 0),
                new Vector3(mixOutRect.xMax, mixOutRect.yMin + 1f, 0), Color.white);
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
