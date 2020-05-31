using UnityEngine;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [Track("动作", true)]
    public class XAnimationTrack : XBindTrack
    {
        public override TrackType trackType
        {
            get { return TrackType.Animation; }
        }

        public XAnimationTrack(BindTrackData data) : base(data)
        {
        }

        public override void Process(float time, float prev)
        {
            base.Process(time, prev);
        }

        protected override IClip BuildClip(ClipData data)
        {
            return new XAnimationClip(this, data);
        }
    }
}
