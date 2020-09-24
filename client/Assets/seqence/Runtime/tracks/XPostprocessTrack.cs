using System;
using UnityEngine.Seqence.Data;

namespace UnityEngine.Seqence
{
    [TrackFlag(TrackFlag.RootOnly)]
    public class XPostprocessTrack : XTrack, ISharedObject<XPostprocessTrack>
    {
        public XPostprocessTrack next { get; set; }

        public override AssetType AssetType
        {
            get { return AssetType.PostProcess; }
        }

        public override bool cloneable
        {
            get { return false; }
        }

        public override XTrack Clone()
        {
            throw new Exception("Postprocess track is uncloneable");
        }

        public override IClip BuildClip(ClipData data)
        {
            var clip = SharedPool<XPostprocessClip>.Get();
            clip.data = (PostprocessData)data;
            clip.track = this;
            clip.OnBuild();
            return clip;
        }

        public override void OnDestroy()
        {
            SharedPool<XPostprocessTrack>.Return(this);
            base.OnDestroy();
        }

        public void Dispose()
        {
            next = null;
        }

        public override string ToString()
        {
            return "Postps " + ID;
        }
    }
}
