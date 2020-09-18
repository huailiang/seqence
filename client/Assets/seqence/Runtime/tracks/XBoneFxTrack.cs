using System;
using UnityEngine.Seqence.Data;

namespace UnityEngine.Seqence
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


        public override IClip BuildClip(ClipData data)
        {
            var clip = SharedPool<XBoneFxClip>.Get();
            clip.data = data;
            clip.track = this;
            return clip;
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
