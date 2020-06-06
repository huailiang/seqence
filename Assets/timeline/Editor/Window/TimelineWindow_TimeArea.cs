using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
    public partial class TimelineWindow
    {
        [NonSerialized] TimeArea m_TimeArea;

        float m_LastFrameRate;
        public Rect timeAreaRect;

        public TimelineState state { get; private set; }

        void InitializeTimeArea()
        {
            if (m_TimeArea == null)
            {
                timeAreaRect = new UnityEngine.Rect(0, 0, winArea.width, WindowConstants.timeAreaHeight);
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
            timeAreaRect.x = winArea.x + WindowConstants.rightAreaMargn;
            timeAreaRect.y = WindowConstants.timeAreaYPosition;
            m_TimeArea.TimeRuler(timeAreaRect, 1, true, false, 1.0f, TimeArea.TimeFormat.Frame);
        }

        void DrawTimeOnSlider()
        {
            float colorDimFactor = EditorGUIUtility.isProSkin ? 0.7f : 0.9f;
            Color c = TimelineStyles.timeCursor.normal.textColor * colorDimFactor;
            float time = state.timeline.Time + 11f;
            time = m_TimeArea.TimeToPixel(time, timeAreaRect);
            Rect rec = new Rect(time, timeAreaRect.y, 2, tree.TracksBtmY);
            EditorGUI.DrawRect(rec, c);
        }

        public float GetSnappedTimeAtMousePosition(Vector2 pos)
        {
            return m_TimeArea.PixelToTime(pos.x, timeAreaRect);
        }

        public float TimeToPixel(float time)
        {
            return m_TimeArea.TimeToPixel(time, timeAreaRect);
        }

        void OnTrackHeadDrag(float newTime)
        {
            state.timeline.Time = Mathf.Max(0.0f, newTime);
        }
    }
}
