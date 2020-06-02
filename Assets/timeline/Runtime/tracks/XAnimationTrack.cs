using UnityEngine.Animations;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [Track("动作", true)]
    public class XAnimationTrack : XBindTrack
    {
        public AnimationPlayableOutput playableOutput;
        private AnimationMixerPlayable mixPlayable;
        private int idx = 0;
        private float tmp = 0;

        public override TrackType trackType
        {
            get { return TrackType.Animation; }
        }

        public XAnimationTrack(XTimeline tl, BindTrackData data) : base(tl, data)
        {
            if (bindObj)
            {
                var amtor = bindObj.GetComponent<Animator>();
                playableOutput = AnimationPlayableOutput.Create(timeline.graph, "AnimationOutput", amtor);
            }
        }

        public override void OnPostBuild()
        {
            base.OnPostBuild();
            if (hasMix)
            {
                mixPlayable = AnimationMixerPlayable.Create(timeline.graph,2);
            }
        }

        protected override IClip BuildClip(ClipData data)
        {
            var clip = new XAnimationClip(this, data);
            clip.port = idx;
            if (tmp > 0 && clip.start < tmp)
            {
                float start = clip.start;
                float duration = tmp - start;
                var mix = new XMixClip<XAnimationTrack>(start, duration, clips[idx - 1], clip);
                AddMix(mix);
            }
            tmp = clip.end;
            idx++;
            return clip;
        }
    }
}
