using System;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XMarkerTrack : XTrack
    {
        public override AssetType AssetType
        {
            get { return AssetType.Marker; }
        }

        public override bool cloneable
        {
            get { return false; }
        }

        public override XTrack Clone()
        {
            throw new Exception("mark track is uncloneable");
        }

        public XMarkerTrack(XTimeline tl, TrackData data) : base(tl, data)
        {
        }

        protected override IClip BuildClip(ClipData data)
        {
            throw new System.Exception("marker no clip");
        }
    }
}
