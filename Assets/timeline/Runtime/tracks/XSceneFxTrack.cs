using UnityEngine;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{

    [Track("场景特效", true)]
    public class XSceneFxTrack : XTrack
    {
        public override TrackType trackType
        {
            get { return TrackType.SceneFx; }
        }

        public XSceneFxTrack(TrackData data) :
            base(data)
        {
        }

        protected override IClip BuildClip(ClipData data)
        {
            return new XSceneFxClip(this, data);
        }

        public override void Process(float time, float prev)
        {
            base.Process(time, prev);
        }
    }
}