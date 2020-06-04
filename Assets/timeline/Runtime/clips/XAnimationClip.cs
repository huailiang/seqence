using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XAnimationClip : XClip<XAnimationTrack>
    {
        public AnimationClipPlayable playable;
        private AnimationClip aclip;
        public int port = 0;


        public XAnimationClip(XAnimationTrack track, ClipData data) : base(track, data)
        {
            AnimClipData anData = data as AnimClipData;
            aclip = Resources.Load<AnimationClip>(anData.anim);
            playable = AnimationClipPlayable.Create(timeline.graph, aclip);
            track.playableOutput.SetSourcePlayable(playable, port);
        }


        protected override void OnUpdate(float tick)
        {
            if (timeline.isRunning)
            {
                playable.SetTime(tick);
            }
            else
            {
                timeline.graph.Evaluate(tick);
            }
        }


        protected override void OnExit()
        {
            port = 0;
            base.OnExit();
        }

        protected override void OnDestroy()
        {
            playable.Destroy();
            Resources.UnloadAsset(aclip);
            base.OnDestroy();
        }
    }
}
