using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [TrackFlag(TrackFlag.RootOnly)]
    public class XLogicValueTrack : XTrack
    {
        public override AssetType AssetType
        {
            get { return AssetType.PostProcess; }
        }

        public override XTrack Clone()
        {
            return new XLogicValueTrack(timeline, data);
        }

        protected override IClip BuildClip(ClipData data)
        {
            return new XLogicClip(this, data);
        }
        
        public XLogicValueTrack(XTimeline tl, TrackData data) : base(tl, data)
        {
        }
    }
}
