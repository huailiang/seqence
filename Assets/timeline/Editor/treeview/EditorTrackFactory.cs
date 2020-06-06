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
                case TrackType.BoneFx:
                    xtrack = new EditorBoneTrack();
                    break;
                case TrackType.PostProcess:
                    xtrack = new EditorPostprocessTrack();
                    break;
                default:
                    xtrack = new EditorTrack();
                    break;
            }
            xtrack.@select = false;
            xtrack.track = track;
            return xtrack;
        }


        public static TrackData CreateData(TrackType type)
        {
            TrackData data = null;
            switch (type)
            {
                case TrackType.Animation:
                    data = new BindTrackData();
                    break;
                default:
                    data = new TrackData();
                    break;
            }
            data.type = type;
            return data;
        }
    }
}
