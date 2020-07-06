using System;
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
        private int idx;
        private float tmp;

        public override AssetType AssetType
        {
            get { return AssetType.Animation; }
        }

        public override bool cloneable
        {
            get { return false; }
        }

        public XAnimationTrack(XTimeline tl, BindTrackData data) : base(tl, data) { }

        protected override IClip BuildClip(ClipData data)
        {
            var clip = new XAnimationClip(this, data);
            clip.port = idx;
            if (tmp > 0 && clip.start < tmp)
            {
                float start = clip.start;
                if (tmp > clip.end) tmp = clip.end - 0.01f;
                float duration = tmp - start;
                var mix = new XMixClip<XAnimationTrack>(start, duration, clips[idx - 1], clip);
                AddMix(mix);
            }
            tmp = clip.end;
            idx++;
            return clip;
        }

        public override XTrack Clone()
        {
            throw new Exception("animation track is uncloneable");
        }


        public AnimationClipPlayable playA, playB;

        protected override void OnMixer(float time, IMixClip mix)
        {
            if (mixPlayable.IsValid())
            {
                if (!mix.connect || !Application.isPlaying)
                {
                    XAnimationClip clipA = (XAnimationClip)mix.blendA;
                    XAnimationClip clipB = (XAnimationClip)mix.blendB;
                    if (clipA && clipB)
                    {
                        playA = clipA.playable;
                        playB = clipB.playable;
                    }
                }
                mix.connect = true;
                float weight = (time - mix.start) / mix.duration;
                if (playA.IsValid() && playB.IsValid())
                {
                    mixPlayable.SetInputWeight(playA, 1 - weight);
                    mixPlayable.SetInputWeight(playB, weight);
                }
                else
                {
                    string tip = "playable invalid while animating mix ";
                    Debug.LogError(tip + playA.IsValid() + " " + playB.IsValid());
                }
            }
        }


        public override void OnBind()
        {
            base.OnBind();
            if (bindObj && XTimeline.graph.IsValid())
            {
                var amtor = bindObj.GetComponent<Animator>();
                if (timeline.IsHostTrack(this))
                {
                    playableOutput = timeline.blendPlayableOutput;
                    mixPlayable = timeline.blendMixPlayable;
                }
                else
                {
                    AnimationTrackData Data = data as AnimationTrackData;
                    bindObj.transform.position = Data.pos;
                    bindObj.transform.rotation = Quaternion.Euler(0, Data.rotY, 0);

                    playableOutput = AnimationPlayableOutput.Create(XTimeline.graph, "AnimationOutput", amtor);
                    mixPlayable = AnimationMixerPlayable.Create(XTimeline.graph);
                }
                playableOutput.SetSourcePlayable(mixPlayable);
            }
        }

        public override void Dispose()
        {
            if (timeline.IsHostTrack(this))
            {
                if (mixPlayable.IsValid())
                {
                    mixPlayable.Destroy();
                }
                if (playableOutput.IsOutputValid())
                {
                    XTimeline.graph.DestroyOutput(playableOutput);
                }
            }
            base.Dispose();
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
