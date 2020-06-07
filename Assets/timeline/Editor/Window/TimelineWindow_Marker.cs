using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public partial class TimelineWindow
    {
        private Rect markderRect;

        void InitializeMarkerHeader()
        {
            markderRect.width = winArea.width;
            markderRect.height = WindowConstants.markerRowHeight;
        }

        void DrawMarkerDrawer()
        {
            if (state.showMarkerHeader)
            {
                DrawMarkerDrawerHeaderBackground();
                DrawMarkerDrawerHeader();
            }
        }

        void DrawMarkerDrawerHeaderBackground()
        {
            var backgroundColor = TimelineStyles.markerHeaderDrawerBackgroundColor;
            markderRect.x = winArea.x + WindowConstants.rightAreaMargn;
            markderRect.y = WindowConstants.markerRowYPosition;
            markderRect.width = winArea.width;
            EditorGUI.DrawRect(markderRect, backgroundColor);
        }

        void DrawMarkerDrawerHeader()
        {
            var tre = state.timeline.trackTrees;
            if (tre != null && tre.Length > 0)
            {
                var marks = tre[0].marks;
                if (marks != null)
                {
                    foreach (var mark in marks)
                    {
                        DrawMarkItem(mark);
                    }
                }
            }
        }

        void DrawMarkItem(XMarker mark)
        {
            float x = TimeToPixel(mark.time);
            Rect rect = markderRect;
            rect.x = x;
            rect.width = 20;
            GUI.Box(rect, m_HeaderContent, TimelineStyles.timeCursor);
        }
    }
}
