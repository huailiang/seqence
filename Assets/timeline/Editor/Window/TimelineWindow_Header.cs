using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow
    {
        void TimelineHeaderGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal(GUILayout.Width(WindowConstants.sliderWidth));
            AddButtonGUI();
            ShowMarkersButton();
            GUILayout.EndHorizontal();
            if (state.showMarkerHeader)
            {
                GUILayout.Label(TimelineStyles.timelineMarkerTrackHeader);
            }
            GUILayout.EndVertical();
        }


        void AddButtonGUI()
        {
            if (EditorGUILayout.DropdownButton(TimelineStyles.addContent, FocusType.Passive, "Dropdown"))
            {
                XTrack parent = null;
                if (parent)
                {
                }
            }

        }


        void ShowMarkersButton()
        {
            var content = state.showMarkerHeader ? TimelineStyles.showMarkersOn : TimelineStyles.showMarkersOff;
            SetShowMarkerHeader(GUILayout.Toggle(state.showMarkerHeader, content, GUI.skin.button));
        }

        internal void SetShowMarkerHeader(bool newValue)
        {
            state.showMarkerHeader = newValue;
        }
    }
}
