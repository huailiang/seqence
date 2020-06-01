using UnityEngine.Animations;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [Track("动作", true)]
    public class XAnimationTrack : XBindTrack
    {
        public AnimationPlayableOutput playableOutput;
        // private AnimationMixerPlayable mixPlayable;
        private int idx = 0;

        public override TrackType trackType
        {
            get { return TrackType.Animation; }
        }

        public XAnimationTrack(BindTrackData data) : base(data)
        {
            if (bindObj)
            {
                var amtor = bindObj.GetComponent<Animator>();
                playableOutput = AnimationPlayableOutput.Create(timeline.graph, "AnimationOutput", amtor);
                // mixPlayable = AnimationMixerPlayable.Create(timeline.graph,2);
            }
        }

        protected override IClip BuildClip(ClipData data)
        {
            var clip = new XAnimationClip(this, data);
            clip.port = idx++;
            return clip;
        }
    }
}
