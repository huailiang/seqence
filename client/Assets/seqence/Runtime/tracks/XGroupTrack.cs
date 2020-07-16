using System;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [TrackFlag(TrackFlag.NoClip)]
    public class XGroupTrack : XTrack, ISharedObject<XGroupTrack>
    {

        public XGroupTrack next { get; set; }


        public override AssetType AssetType
        {
            get { return AssetType.Group; }
        }

        public override XTrack Clone()
        {
            return XSeqenceFactory.GetTrack(data, seqence, parent);
        }

        public override IClip BuildClip(ClipData data)
        {
            throw new Exception("Group no clip");
        }

        public override string ToString()
        {
            return "Track Group";
        }

        public override void OnDestroy()
        {
            SharedPool<XGroupTrack>.Return(this);
            base.OnDestroy();
        }

        public void Dispose()
        {
            next = null;
        }
    }
}
