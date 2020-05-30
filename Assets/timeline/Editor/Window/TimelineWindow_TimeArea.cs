using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow
    {
        [NonSerialized] TimelineTimeArea m_TimeArea;

        public TimeArea timeArea
        {
            get  { return m_TimeArea; }
        }

        internal static class Styles
        {
            public static string DurationModeText = L10n.Tr("Duration Mode/{0}");
        }

        float m_LastFrameRate;
        Rect timeAreaRect;

        void InitializeTimeArea()
        {
            if (m_TimeArea == null)
            {
                timeAreaRect = new UnityEngine.Rect(0, 0, winArea.width, WindowConstants.timeAreaHeight);
                m_TimeArea = new TimelineTimeArea(false)
                {
                    hRangeLocked = false,
                    vRangeLocked = true,
                    margin = 10,
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
            timeAreaRect.y = m_TimeArea.topmargin + WindowConstants.timeAreaYPosition;
            m_TimeArea.TimeRuler(timeAreaRect, 3, true, false, 1.0f, TimeArea.TimeFormat.Frame);
        }


        class TimelineTimeArea : TimeArea
        {
            public TimelineTimeArea(bool minimalGUI) : base(minimalGUI)
            {
            }
        }
    }
}
