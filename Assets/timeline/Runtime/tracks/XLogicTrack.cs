using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [TrackFlag(TrackFlag.SubOnly)]
    [UseParent(typeof(XAnimationTrack))]
    public class XLogicTrack : XTrack
    {
        public TimelineDraw draw;

        public override AssetType AssetType
        {
            get { return AssetType.LogicValue; }
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
            InitDraw();
        }

        private void InitDraw()
        {
            draw = new TimelineDraw();
        }

        public void Clean()
        {
             draw?.Clean();
        }


        public override void Dispose()
        {
            draw?.Destroy();
            base.Dispose();
        }
    }
}
