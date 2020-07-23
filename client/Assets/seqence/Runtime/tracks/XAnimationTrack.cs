using System;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline.Data;
using Unity.Collections;
#if UNITY_2019_3_OR_NEWER
using UnityEngine.Animations;
#else
using UnityEngine.Experimental.Animations;
#endif

namespace UnityEngine.Timeline
{
    [TrackRequreType(typeof(Animator))]
    [TrackFlag(TrackFlag.RootOnly)]
    public class XAnimationTrack : XBindTrack, ISharedObject<XAnimationTrack>
    {
        public AnimationPlayableOutput playableOutput;
        public AnimationScriptPlayable mixPlayable;
        public MixerJob mixJob;
        private int idx;
        private float tmp;

        public XAnimationTrack next { get; set; }

        public override AssetType AssetType
        {
            get { return AssetType.Animation; }
        }

        public override bool cloneable
        {
            get { return false; }
        }

        public override IClip BuildClip(ClipData data)
        {
            var clip = SharedPool<XAnimationClip>.Get();
            clip.data = data;
            clip.track = this;
            clip.Initial(data, idx);
            if (tmp > 0 && clip.start < tmp)
            {
                float start = clip.start;
                if (tmp > clip.end) tmp = clip.end - 0.01f;
                float duration = tmp - start;
                BuildMix(start, duration, clips[idx - 1], clip);
            }
            tmp = clip.end;
            idx++;
            return clip;
        }

        public override XTrack Clone()
        {
            throw new Exception("animation track is uncloneable");
        }


        private AnimationClipPlayable mixA, mixB;

        protected override void OnMixer(float time, MixClip mix)
        {
            if (mixPlayable.IsValid())
            {
                if (!mix.connect || !Application.isPlaying)
                {
                    XAnimationClip clipA = (XAnimationClip) mix.blendA;
                    XAnimationClip clipB = (XAnimationClip) mix.blendB;
                    if (clipA && clipB)
                    {
                        mixA = clipA.playable;
                        mixB = clipB.playable;
                    }
                }
                mix.connect = true;
                float weight = (time - mix.start) / mix.duration;
                if (mixA.IsValid() && mixB.IsValid())
                {
                    mixJob.weight = weight;
                    mixPlayable.SetJobData(mixJob);
                }
                else
                {
                    string tip = "playable invalid while animating mix ";
                    Debug.LogError(tip + mixA.IsValid() + " " + mixB.IsValid());
                }
            }
        }


        public override void OnBind()
        {
            if (bindObj && XSeqence.graph.IsValid())
            {
                var amtor = bindObj.GetComponent<Animator>();
                var transforms = amtor.transform.GetComponentsInChildren<Transform>();
                var numTransforms = transforms.Length - 1;

                if (seqence.IsHostTrack(this) && seqence.blendPlayableOutput.IsOutputValid())
                {
                    playableOutput = seqence.blendPlayableOutput;
                    mixPlayable = seqence.blendMixPlayable;
                    mixJob = seqence.mixJob;
                }
                else
                {
                    var handles = new NativeArray<TransformStreamHandle>(numTransforms, Allocator.Persistent,
                        NativeArrayOptions.UninitializedMemory);
                    for (var i = 0; i < numTransforms; ++i)
                    {
                        handles[i] = amtor.BindStreamTransform(transforms[i + 1]);
                    }
                    mixJob = new MixerJob() {handles = handles, weight = 1.0f};

                    AnimationTrackData Data = data as AnimationTrackData;
                    bindObj.transform.position = Data.pos;
                    bindObj.transform.rotation = Quaternion.Euler(0, Data.rotY, 0);

                    playableOutput = AnimationPlayableOutput.Create(XSeqence.graph, "AnimationOutput", amtor);
                    mixPlayable = AnimationScriptPlayable.Create(XSeqence.graph, mixJob);
                    mixPlayable.SetProcessInputs(false);
                }
                playableOutput.SetSourcePlayable(mixPlayable);
            }
            base.OnBind();
        }

        public override void Process(float time, float prev)
        {
            base.Process(time, prev);
            if (mixPlayable.IsValid())
            {
                mixJob.clipA = clipA;
                mixJob.clipB = clipB;
                mixPlayable.SetJobData(mixJob);
            }
        }


        public override void OnDestroy()
        {
            if (!seqence.IsHostTrack(this))
            {
                if (mixPlayable.IsValid())
                {
                    mixPlayable.Destroy();
                }
                if (playableOutput.IsOutputValid())
                {
                    XSeqence.graph.DestroyOutput(playableOutput);
                }
                mixJob.Dispose();
                SharedPool<XAnimationTrack>.Return(this);
                idx = 0;
                tmp = 0;
            }
            base.OnDestroy();
        }

        public void Dispose()
        {
            next = null;
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
