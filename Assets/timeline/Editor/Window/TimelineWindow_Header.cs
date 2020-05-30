using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow
    {

        void TimelineHeaderGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.BeginHorizontal(GUILayout.Width(WindowConstants.sliderWidth));
            AddButtonGUI();
            GUILayout.Space(8);
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
                //var groupTracks = SelectionManager.SelectedTracks().ToList();
                //if (groupTracks.Count == 1)
                //{
                //    parent = groupTracks[0] as GroupTrack;
                //    // if it's locked, add to the root instead
                //    if (parent != null && parent.lockedInHierarchy)
                //        parent = null;
                //}
                //SequencerContextMenu.ShowNewTracksContextMenu(parent, null, state);
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
