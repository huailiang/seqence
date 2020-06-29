using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [TrackFlag(TrackFlag.RootOnly)]
    public class XLogicTrack : XTrack
    {
        public override AssetType AssetType
        {
            get { return AssetType.PostProcess; }
        }

        public override XTrack Clone()
        {
            return new XLogicTrack(timeline, data);
        }

        protected override IClip BuildClip(ClipData data)
        {
            return new XLogicClip(this, data);
        }
        
        public XLogicTrack(XTimeline tl, TrackData data) : base(tl, data)
        {
        }
    }
}
