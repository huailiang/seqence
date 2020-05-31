using UnityEngine;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{

    [Track("骨骼特效", false)]
    public class XBoneFxTrack : XTrack
    {

        public GameObject target;

        public override TrackType trackType
        {
            get { return TrackType.BoneFx; }
        }

        public XBoneFxTrack(TrackData data) :
            base(data)
        {
        }

        protected override IClip BuildClip(ClipData data)
        {
            return new XBoneFxClip(this, data);
        }


        public override void Process(float time, float prev)
        {
            base.Process(time, prev);
        }

    }

}