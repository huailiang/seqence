using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    
    [TimelineEditor(typeof(XLogicValueTrack))]
    public class EditorLogicTrack: EditorTrack
    {
        protected override Color trackColor
        {
            get { return Color.blue; }
        }

        protected override string trackHeader
        {
            get { return "打击点" + ID; }
        }

        protected override void OnAddClip(float t)
        {
            LogicClipData clipData = new LogicClipData();
            clipData.start = t;
            clipData.duration = 16;
            XLogicClip clip = new XLogicClip((XLogicValueTrack) track, clipData);
            track.AddClip(clip);
        }
    }
}
