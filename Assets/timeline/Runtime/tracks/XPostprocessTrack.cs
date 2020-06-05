using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [Track("后处理", false)]
    public class XPostprocessTrack : XTrack
    {
        
        public override TrackType trackType
        {
            get { return TrackType.PostProcess; }
        }

        protected override IClip BuildClip(ClipData data)
        {
            throw new System.NotImplementedException();
        }

        public XPostprocessTrack(XTimeline tl, TrackData data) : base(tl, data)
        {
        }

    }
}