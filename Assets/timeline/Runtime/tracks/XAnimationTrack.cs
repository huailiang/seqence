using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [TrackRequreType(typeof(Animator))]
    [TrackFlag(TrackFlag.RootOnly)]
    public class XAnimationTrack : XBindTrack
    {
        public AnimationPlayableOutput playableOutput;
        public AnimationMixerPlayable mixPlayable;
        private int idx = 0;
        private float tmp = 0;

        public override AssetType AssetType
        {
            get { return AssetType.Animation; }
        }

        public override XTrack Clone()
        {
            return new XAnimationTrack(timeline, (BindTrackData) data);
        }

        public XAnimationTrack(XTimeline tl, BindTrackData data) : base(tl, data)
        {
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

        private AnimationClipPlayable playA, playB;

        protected override void OnMixer(float time, IMixClip mix)
        {
            if (mixPlayable.IsValid())
            {
                if (!mix.connect)
                {
                    XAnimationClip clipA = (XAnimationClip) mix.blendA;
                    XAnimationClip clipB = (XAnimationClip) mix.blendB;
                    if (clipA && clipB)
                    {
                        playA = clipA.playable;
                        playB = clipB.playable;
                    }
                }
                mix.connect = true;
                float weight = (time - mix.start) / mix.duration;
                if (playA.IsValid()) mixPlayable.SetInputWeight(playA, 1 - weight);
                if (playB.IsValid()) mixPlayable.SetInputWeight(playB, weight);
            }
        }

        public override void OnBind()
        {
            base.OnBind();
            if (bindObj)
            {
                var amtor = bindObj.GetComponent<Animator>();
                playableOutput = AnimationPlayableOutput.Create(timeline.graph, "AnimationOutput", amtor);
                mixPlayable = AnimationMixerPlayable.Create(timeline.graph);
                playableOutput.SetSourcePlayable(mixPlayable);
            }
        }

        public override string ToString()
        {
            if (bindObj)
            {
                return bindObj + " " + ID;
            }
            else
            {
                return "Animator " + ID;
            }
        }
    }
}
