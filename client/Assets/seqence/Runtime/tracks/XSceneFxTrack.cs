using System;
using UnityEngine.Seqence.Data;

namespace UnityEngine.Seqence
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


        public override IClip BuildClip(ClipData data)
        {
            var clip = SharedPool<XSceneFxClip>.Get();
            clip.data = (SceneFxClipData)data;
            clip.track = this;
            SceneFxClipData fxdata = (SceneFxClipData)data;
            clip.Load(fxdata.prefab, fxdata.pos, fxdata.rot, fxdata.scale);
            return clip;
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
