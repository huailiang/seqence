using UnityEngine;

namespace UnityEditor.Timeline
{
    public class TimelineMarkerHeaderGUI
    {
        static Rect bgRect = new Rect(0,0,100,40);
        
        static void DrawMarkerDrawerHeaderBackground()
        {
            var backgroundColor = DirectorNamedColor.markerHeaderDrawerBackgroundColor;
            var bgRect = new Rect();
            EditorGUI.DrawRect(bgRect, backgroundColor);
        }
    }
}