using System;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [TrackFlag(TrackFlag.SubOnly)]
    [UseParent(typeof(XAnimationTrack))]
    public class XBoneFxTrack : XTrack, ISharedObject<XBoneFxTrack>
    {

        public XBoneFxTrack next { get; set; }

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


        protected override IClip BuildClip(ClipData data)
        {
            return new XBoneFxClip(this, data);
        }


        public override string ToString()
        {
            return "BoneFx " + ID;
        }


        public override void OnDestroy()
        {
            SharedPool<XBoneFxTrack>.Return(this);
            base.OnDestroy();
        }

        public void Dispose()
        {
            next = null;
        }
    }
}
