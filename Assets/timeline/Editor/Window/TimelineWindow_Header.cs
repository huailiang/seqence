using System;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    public partial class TimelineWindow
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
                GenericMenu pm = new GenericMenu();
                var types = TypeUtilities.AllRootTrackExcMarkers();
                for (int i = 0; i < types.Count; i++)
                {
                    string str = types[i].ToString();
                    int idx = str.LastIndexOf('.');
                    if (idx >= 0)
                    {
                        str = str.Substring(idx + 1);
                    }
                    pm.AddItem(EditorGUIUtility.TrTextContent(str), false, OnAddTrackItem, types[i]);
                }
                Rect rect = new Rect(Event.current.mousePosition, new Vector2(200, 0));
                pm.DropDown(rect);
            }
        }

        private void OnAddTrackItem(object arg)
        {
            Type type = (Type) arg;
            TrackData data = XTimelineFactory.CreateTrackData(type);
            var track = XTimelineFactory.GetTrack(data, state.timeline);
            tree.AddTrack(track);
            state.timeline.AddRootTrack(track);
        }

        void ShowMarkersButton()
        {
            var content = state.showMarkerHeader ? TimelineStyles.showMarkersOn : TimelineStyles.showMarkersOff;
            bool b = state.showMarkerHeader;
            SetShowMarkerHeader(GUILayout.Toggle(state.showMarkerHeader, content, GUI.skin.button));
            if (b != state.showMarkerHeader)
            {
                tree.MarksOffset(state.showMarkerHeader);
            }
        }

        internal void SetShowMarkerHeader(bool newValue)
        {
            state.showMarkerHeader = newValue;
        }
    }
}
