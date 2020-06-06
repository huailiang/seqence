using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XTrackFactory
    {
        public static XTrack Get(TrackData data, XTimeline tl)
        {
            XTrack xTrack = null;
            switch (data.type)
            {
                case TrackType.Marker:
                    xTrack = new XMarkerTrack(tl, data);
                    break;
                case TrackType.Animation:
                    xTrack = new XAnimationTrack(tl, data as BindTrackData);
                    break;
                case TrackType.BoneFx:
                    xTrack = new XBoneFxTrack(tl, data);
                    break;
                case TrackType.SceneFx:
                    xTrack = new XSceneFxTrack(tl, data);
                    break;
                case TrackType.PostProcess:
                    xTrack =new XPostprocessTrack(tl,data);
                    break;
                default:
                    Debug.LogError("unknown track " + data.type);
                    break;
            }
            if (xTrack)
            {
                xTrack.OnPostBuild();
            }
            return xTrack;
        }
    }
}
