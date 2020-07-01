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
        private float rangeX1, rangeX2;
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
            rangeX1 = 0;
            rangeX2 = 60;
        }

        public void TimelineTimeAreaGUI()
        {
            timeAreaRect.width = winArea.width;
            timeAreaRect.x = WindowConstants.rightAreaMargn;
            timeAreaRect.y = WindowConstants.timeAreaYPosition;
            m_TimeArea.TimeRuler(timeAreaRect, 30, true, false, 1.0f, TimeArea.TimeFormat.TimeFrame);
            m_TimeArea.SetShownHRange(rangeX1, rangeX2);
        }

        void DrawTimeOnSlider()
        {
            float colorDimFactor = EditorGUIUtility.isProSkin ? 0.7f : 0.9f;
            Color c = TimelineStyles.timeCursor.normal.textColor * colorDimFactor;
            float time = state.timeline.Time;
            time = m_TimeArea.TimeToPixel(time, timeAreaRect);
            float h = tree.TracksBtmY - timeAreaRect.y - 2;
            Rect rec = new Rect(time, timeAreaRect.y, 2, h);
            if (IsPiexlRange(time))
            {
                EditorGUI.DrawRect(rec, c);
                rec.height = timeAreaRect.height;
                rec.x -= 4;
                rec.width = 20;
                GUI.Box(rec, TimelineStyles.empty, TimelineStyles.timeCursor);
            }
            if (e == null) e = Event.current;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (rec.Contains(e.mousePosition))
                    {
                        time_draging = true;
                    }
                    else if (timeAreaRect.Contains(e.mousePosition))
                    {
                        float t = PiexlToTime(e.mousePosition.x);
                        timeline.Time = t;
                        Repaint();
                    }
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
                    else if (timeAreaRect.Contains(e.mousePosition)) //zoom
                    {
                        var dt = e.delta;
                        float delta = Mathf.Abs(dt.x) > Mathf.Abs(dt.y) ? dt.x : dt.y;
                        if (Mathf.Abs(delta) > 1e-5)
                        {
                            delta = Mathf.Clamp(delta, -0.01f, 0.01f);
                            float sc = 1 + delta;
                            float center = PiexlToTime(e.mousePosition.x);
                            rangeX1 = center - sc * (center - rangeX1);
                            rangeX2 = center + sc * (rangeX2 - center);
                            m_TimeArea.SetShownHRange(rangeX1, rangeX2);
                            Repaint();
                        }
                    }
                    break;
            }
        }

        public void SetTimeRange(float x1, float x2)
        {
            rangeX1 = x1;
            rangeX2 = x2;
            m_TimeArea.SetShownHRange(rangeX1, rangeX2);
            Repaint();
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

        public bool IsTimeRange(float t)
        {
            return t >= rangeX1 && t <= rangeX2;
        }

        public bool IsPiexlRange(float piexl)
        {
            float t = PiexlToTime(piexl);
            return IsTimeRange(t);
        }

        void OnTrackHeadDrag(float newTime)
        {
            state.playing = false;
            state.timeline.Time = Mathf.Max(0.0f, newTime);
            TimelineWindow.inst.Repaint();
        }
    }
}
