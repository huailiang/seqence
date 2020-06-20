using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [TrackFlag(TrackFlag.RootOnly)]
    public class XSceneFxTrack : XTrack
    {
        public override AssetType AssetType
        {
            get { return AssetType.SceneFx; }
        }

        public override XTrack Clone()
        {
            return new XSceneFxTrack(timeline, data);
        }

        public XSceneFxTrack(XTimeline tl, TrackData data) : base(tl, data)
        {
        }

        protected override IClip BuildClip(ClipData data)
        {
            return new XSceneFxClip(this, data);
        }

        public override string ToString()
        {
            return "SceneFx " + ID;
        }
    }
}
