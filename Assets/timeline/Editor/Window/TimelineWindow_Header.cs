using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Timeline
{

    partial class TimelineWindow
    {

        void TimelineHeaderGUI()
        {
            GUILayout.BeginHorizontal(GUILayout.Width(80));
            AddButtonGUI();
            ShowMarkersButton();
            GUILayout.EndHorizontal();
        }


        void AddButtonGUI()
        {
            if (EditorGUILayout.DropdownButton(TimelineStyles.addContent, FocusType.Passive, "Dropdown"))
            {
                // if there is 1 and only 1 track selected, AND it's a group, add to that group
                //TrackAsset parent = null;
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
