using System;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [TrackDescriptor("后处理", false)]
    public class XPostprocessTrack : XTrack
    {
        public override TrackType trackType
        {
            get { return TrackType.PostProcess; }
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
