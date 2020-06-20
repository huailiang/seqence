using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XAnimationClip : XClip<XAnimationTrack>
    {
        public AnimationClipPlayable playable;
        public AnimationClip aclip;
        public int port = 0;

        public override string Display
        {
            get { return aclip != null ? aclip.name + " " + port : " anim:" + port; }
        }

        public XAnimationClip(XAnimationTrack track, ClipData data) : base(track, data)
        {
            AnimClipData anData = data as AnimClipData;
            aclip = XResources.LoadSharedAsset<AnimationClip>(anData.anim);
            playable = AnimationClipPlayable.Create(timeline.graph, aclip);
            RebindPlayable();
        }

        public void RebindPlayable()
        {
            if (track.playableOutput.IsOutputValid())
            {
                track.playableOutput.SetSourcePlayable(playable, port);
            }
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
            AnimClipData anData = data as AnimClipData;
            XResources.DestroySharedAsset(anData.anim);
            base.OnDestroy();
        }
    }
}
