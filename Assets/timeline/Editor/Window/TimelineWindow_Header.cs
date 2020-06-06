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
                XTrack parent = tree.GetSelectTrack();
                if (parent)
                {
                    var track = parent.Clone();
                    track.parent = parent;
                    int idx = tree.IndexOfTrack(parent);
                    tree.AddTrack(track, ++idx);
                    parent.AddSub(track);
                }
                else
                {
                    BindTrackData data = new BindTrackData();
                    data.type = TrackType.Animation;
                    var track = XTrackFactory.Get(data, state.timeline);
                    tree.AddTrack(track);
                    state.timeline.AddRootTrack(track);
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
