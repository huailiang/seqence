using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow
    {
        [NonSerialized] TimeArea m_TimeArea;

        float m_LastFrameRate;
        Rect timeAreaRect;

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
            timeAreaRect.y = m_TimeArea.topmargin + WindowConstants.timeAreaYPosition;
            m_TimeArea.TimeRuler(timeAreaRect, 1, true, false, 1.0f, TimeArea.TimeFormat.Frame);
            
            DrawTimeOnSlider();
            DrawTimeCursor();
        }
        
        void DrawTimeOnSlider()
        {
            float colorDimFactor = EditorGUIUtility.isProSkin ? 0.7f : 0.9f;
            Color c = TimelineStyles.timeCursor.normal.textColor * colorDimFactor;
            float time = Mathf.Max(state.timeline?.Time ?? 0, 2.0f);
            float duration = state.timeline?.RecalcuteDuration() ?? 5.0f;
            m_TimeArea.DrawTimeOnSlider(time, c, duration, TimelineStyles.kDurationGuiThickness);
        }

        void DrawTimeCursor()
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                if (timeAreaRect.Contains(Event.current.mousePosition))
                {
                    state.playing = false;
                    if (state.timeline)
                    {
                        state.timeline.Time =
                            Mathf.Max(0.0f, GetSnappedTimeAtMousePosition(Event.current.mousePosition));
                    }
                }
            }
        }

        public float GetSnappedTimeAtMousePosition(Vector2 pos)
        {
            Debug.Log(pos);
            return 0.0f;
        }


        void OnTrackHeadDrag(float newTime)
        {
            state.timeline.Time = Mathf.Max(0.0f, newTime);
        }
        
    }
}
