using System;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [TrackFlag(TrackFlag.RootOnly)]
    public class XPostprocessTrack : XTrack
    {
        public override AssetType AssetType
        {
            get { return AssetType.PostProcess; }
        }

        public override bool cloneable
        {
            get { return false; }
        }

        public override XTrack Clone()
        {
            throw new Exception("Postprocess track is uncloneable");
        }

        protected override IClip BuildClip(ClipData data)
        {
            return new XPostprocessClip(this, data);
        }

        public XPostprocessTrack(XTimeline tl, TrackData data) : base(tl, data)
        {
        }

        public override string ToString()
        {
            return "Postps " + ID;
        }
    }
}
