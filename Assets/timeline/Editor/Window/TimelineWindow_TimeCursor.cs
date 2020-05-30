using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow
    {
        void TimeCursorGUI()
        {
            DrawTimeOnSlider();
            DrawTimeCursor();
        }

        void DrawTimeOnSlider()
        {
            float colorDimFactor = EditorGUIUtility.isProSkin ? 0.7f : 0.9f;
            Color c = TimelineStyles.timeCursor.normal.textColor * colorDimFactor;
            float time = Mathf.Max(state.timeline?.Time ?? 0, 5.0f);
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
