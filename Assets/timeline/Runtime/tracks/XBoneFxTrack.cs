using System;
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
                if (parent && parent is XBindTrack track)
                {
                    return track.bindObj;
                }
                return null;
            }
        }

        public override AssetType AssetType
        {
            get { return AssetType.BoneFx; }
        }

        public override bool cloneable
        {
            get { return false; }
        }

        public override XTrack Clone()
        {
            throw new Exception("bonefx track is uncloneable");
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
