using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [TrackDescriptor("骨骼特效", true)]
    public class XBoneFxTrack : XTrack
    {
        public GameObject target;
        private TrackData data;

        public override TrackType trackType
        {
            get { return TrackType.BoneFx; }
        }

        public override XTrack Clone()
        {
            return new XBoneFxTrack(timeline, data);
        }

        public XBoneFxTrack(XTimeline tl, TrackData data) : base(tl, data)
        {
            this.data = data;
        }

        protected override IClip BuildClip(ClipData data)
        {
            return new XBoneFxClip(this, data);
        }

        public override string ToString()
        {
            return "BoneFx " + ID;
        }
    }
}
