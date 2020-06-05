using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public class TimelineTrackGUI : TimelineTrackBaseGUI
    {
        public override Rect boundingRect
        {
            get { return new Rect(); }
        }

        public TimelineTrackGUI(int id, int depth, TreeViewItem parent, string displayName, XTrack trackAsset) : base(
            id, depth, parent, displayName, trackAsset)
        {
        }

        public TimelineTrackGUI(XTrack track) : base(track)
        {
        }


        public override void Draw(Rect headerRect, Rect contentRect)
        {
        }

        public override void OnGraphRebuilt()
        {
        }
    }
}
