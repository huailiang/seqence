using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow
    {
        [NonSerialized] TimelineTimeArea m_TimeArea;
        public TimeArea timeArea { get { return m_TimeArea; } }

        internal static class Styles
        {
            public static string DurationModeText = L10n.Tr("Duration Mode/{0}");
        }

        float m_LastFrameRate;
        Rect rect;

        void InitializeTimeArea()
        {
            if (m_TimeArea == null)
            {
                rect = new UnityEngine.Rect(10, 10, winArea.width, 18);
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
                    rect = rect,
                };
            }
        }

        public void TimelineTimeAreaGUI()
        {
            rect.width = winArea.width;
            rect.y = m_TimeArea.topmargin + 12;
            m_TimeArea.TimeRuler(rect, 3, true, false, 1.0f, TimeArea.TimeFormat.Frame);
        }


        class TimelineTimeArea : TimeArea
        {

            public TimelineTimeArea(bool minimalGUI) : base(minimalGUI)
            {

            }
        }
    }
}
