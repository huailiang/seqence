using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [TrackFlag(TrackFlag.SubOnly)]
    [UseParent(typeof(XAnimationTrack))]
    public class XLogicTrack : XTrack, ISharedObject<XLogicTrack>
    {
        public SeqenceDraw draw;

        public XLogicTrack next { get; set; }

        public override AssetType AssetType
        {
            get { return AssetType.LogicValue; }
        }

        public override XTrack Clone()
        {
            TrackData data = CloneData();
            return XSeqenceFactory.GetTrack(data, seqence, parent);
        }

        public override IClip BuildClip(ClipData data)
        {
            var clip = SharedPool<XLogicClip>.Get();
            clip.data = data;
            clip.track = this;
            return clip;
        }

        protected override void OnPostBuild()
        {
            base.OnPostBuild();
            InitDraw();
        }

        private void InitDraw()
        {
            draw = new SeqenceDraw();
        }

        public void Clean()
        {
            draw?.Clean();
        }


        public override void OnDestroy()
        {
            draw?.Destroy();
            base.OnDestroy();
            SharedPool<XLogicTrack>.Return(this);
        }

        public void Dispose()
        {
            next = null;
        }
    }
}
