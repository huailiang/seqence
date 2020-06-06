using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    public class EditorTrackFactory
    {
        public static EditorTrack Get(XTrack track)
        {
            EditorTrack xtrack = null;
            switch (track.trackType)
            {
                case TrackType.Animation:
                    xtrack = new EditorAnimTrack();
                    break;
                case TrackType.SceneFx:
                    xtrack = new EditorSceneFxTrack();
                    break;
                default:
                    xtrack = new EditorTrack();
                    break;
            }
            xtrack.@select = false;
            xtrack.track = track;
            return xtrack;
        }
    }
}
