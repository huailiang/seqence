using System;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [TrackFlag(TrackFlag.RootOnly)]
    public class XSceneFxTrack : XTrack, ISharedObject<XSceneFxTrack>
    {
        public XSceneFxTrack next { get; set; }

        public override AssetType AssetType
        {
            get { return AssetType.SceneFx; }
        }

        public override bool cloneable
        {
            get { return false; }
        }

        public override XTrack Clone()
        {
            throw new Exception("scenefx track is uncloneable");
        }


        protected override IClip BuildClip(ClipData data)
        {
            return new XSceneFxClip(this, data);
        }

        public override string ToString()
        {
            return "SceneFx " + ID;
        }

        public override void OnDestroy()
        {
            SharedPool<XSceneFxTrack>.Return(this);
            base.OnDestroy();
        }

        public void Dispose()
        {
            next = null;
        }
    }
}
