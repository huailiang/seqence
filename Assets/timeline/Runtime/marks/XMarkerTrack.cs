using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XMarkerTrack : XTrack
    {
        public override TrackType trackType
        {
            get { return TrackType.Marker; }
        }

        public XMarkerTrack(TrackData data) : base(data)
        {
        }

        protected override IClip BuildClip(ClipData data)
        {
            throw new System.Exception("marker no clip");
        }

       
    }
}