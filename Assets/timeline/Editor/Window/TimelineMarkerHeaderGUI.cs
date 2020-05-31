using UnityEngine;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow
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
            markderRect.y = m_TimeArea.topmargin + WindowConstants.markerRowYPosition;
            markderRect.width = winArea.width;
            EditorGUI.DrawRect(markderRect, backgroundColor);
        }

        void DrawMarkerDrawerHeader()
        {
        }
        
    }
}
