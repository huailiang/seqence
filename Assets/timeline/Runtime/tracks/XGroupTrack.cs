using System;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [TrackFlag(TrackFlag.NoClip)]
    public class XGroupTrack : XTrack
    {
        public XGroupTrack(XTimeline tl, TrackData data) : base(tl, data)
        {
        }

        public override AssetType AssetType
        {
            get { return AssetType.Group; }
        }

        public override XTrack Clone()
        {
            return new XGroupTrack(timeline, data);
        }

        protected override IClip BuildClip(ClipData data)
        {
            throw new Exception("Group no clip");
        }

        public override string ToString()
        {
            return "Track Group";
        }
    }
}
