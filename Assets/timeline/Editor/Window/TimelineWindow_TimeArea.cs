using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
    public partial class TimelineWindow
    {
        [NonSerialized] TimeArea m_TimeArea;

        float m_LastFrameRate;
        public Rect timeAreaRect;
        private bool time_draging;
        public TimelineState state { get; private set; }

        void InitializeTimeArea()
        {
            if (m_TimeArea == null)
            {
                time_draging = false;
                timeAreaRect.height = WindowConstants.timeAreaHeight;
                m_TimeArea = new TimeArea(false)
                {
                    hRangeLocked = false,
                    vRangeLocked = true,
                    margin = 0,
                    scaleWithWindow = true,
                    hSlider = true,
                    vSlider = false,
                    hBaseRangeMin = 0.0f,
                    hRangeMin = 0.0f,
                    rect = timeAreaRect,
                };
            }
        }

        public void TimelineTimeAreaGUI()
        {
            timeAreaRect.width = winArea.width;
            timeAreaRect.x = WindowConstants.rightAreaMargn;
            timeAreaRect.y = WindowConstants.timeAreaYPosition;
            m_TimeArea.TimeRuler(timeAreaRect, 1, true, false, 1.0f, TimeArea.TimeFormat.Frame);
        }

        void DrawTimeOnSlider()
        {
            float colorDimFactor = EditorGUIUtility.isProSkin ? 0.7f : 0.9f;
            Color c = TimelineStyles.timeCursor.normal.textColor * colorDimFactor;
            float time = state.timeline.Time + 0.1f;
            time = m_TimeArea.TimeToPixel(time, timeAreaRect);
            Rect rec = new Rect(time, timeAreaRect.y, 2, tree.TracksBtmY);
            EditorGUI.DrawRect(rec, c);
            rec.height = timeAreaRect.height;
            rec.x -= 4;
            rec.width = 20;
            GUI.Box(rec, TimelineStyles.empty, TimelineStyles.timeCursor);

            if (e == null) e = Event.current;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (rec.Contains(e.mousePosition))
                        time_draging = true;
                    break;
                case EventType.MouseUp:
                    time_draging = false;
                    break;
                case EventType.ScrollWheel:
                case EventType.MouseDrag:
                    if (time_draging)
                    {
                        float xtime = m_TimeArea.PixelToTime(e.mousePosition.x, timeAreaRect);
                        OnTrackHeadDrag(xtime);
                    }
                    break;
            }
        }

        public float GetSnappedTimeAtMousePosition(Vector2 pos)
        {
            return m_TimeArea.PixelToTime(pos.x, timeAreaRect);
        }

        public float TimeToPixel(float time)
        {
            return m_TimeArea.TimeToPixel(time, timeAreaRect);
        }

        public float PiexlToTime(float piexl)
        {
            return m_TimeArea.PixelToTime(piexl, timeAreaRect);
        }

        void OnTrackHeadDrag(float newTime)
        {
            state.timeline.Time = Mathf.Max(0.0f, newTime);
            TimelineWindow.inst.Repaint();
        }
    }
}
