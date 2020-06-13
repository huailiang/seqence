using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [TrackDescriptor(true)]
    [UseParent(typeof(XAnimationTrack))]
    public class XBoneFxTrack : XTrack
    {
        private TrackData data;

        public GameObject target
        {
            get
            {
                if (parent && parent is XBindTrack)
                {
                    return (parent as XBindTrack).bindObj;
                }
                return null;
            }
        }

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
