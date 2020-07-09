using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    [TimelineEditor(typeof(XPostprocessTrack))]
    public class EditorPostprocessTrack : EditorTrack
    {
        protected override Color trackColor
        {
            get { return Color.magenta; }
        }

        protected override string trackHeader
        {
            get { return "后处理" + ID; }
        }

        protected override bool warn
        {
            get { return track.clips == null; }
        }

        protected override void OnAddClip(float t)
        {
            PostprocessData data = new PostprocessData();
            data.start = t;
            data.duration = 20;
            var clip = track.BuildClip(data);
            track.AddClip(clip, data);
        }

        protected override void OnInspectorTrack()
        {
            base.OnInspectorTrack();
            if(track.clips==null)
            {
                EditorGUILayout.HelpBox("There is no clip in track", MessageType.Warning);
            }
        }
    }
}
