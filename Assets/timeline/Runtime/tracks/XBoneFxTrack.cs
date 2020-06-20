using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [TrackFlag(TrackFlag.SubOnly)]
    [UseParent(typeof(XAnimationTrack))]
    public class XBoneFxTrack : XTrack
    {
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

        public override AssetType AssetType
        {
            get { return AssetType.BoneFx; }
        }

        public override XTrack Clone()
        {
            return new XBoneFxTrack(timeline, data);
        }

        public XBoneFxTrack(XTimeline tl, TrackData data) : base(tl, data)
        {
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
