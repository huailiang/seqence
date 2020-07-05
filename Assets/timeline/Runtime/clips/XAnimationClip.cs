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
        private AnimClipData anData;

        public override string Display
        {
            get { return aclip != null ? aclip.name + " " + port : " anim:" + port; }
        }

        public XAnimationClip(XAnimationTrack track, ClipData data) : base(track, data)
        {
            anData = data as AnimClipData;
            aclip = XResources.LoadSharedAsset<AnimationClip>(anData.anim);
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            if (track.mixPlayable.IsValid())
            {
                int cnt = track.mixPlayable.GetInputCount() + 1;
                track.mixPlayable.SetInputCount(cnt);
                playable = AnimationClipPlayable.Create(XTimeline.graph, aclip);
                if (playable.IsValid())
                {
                    XTimeline.graph.Connect(playable, 0, track.mixPlayable, port);
                    track.mixPlayable.SetInputWeight(port, 1);
                }
                else
                {
                    Debug.LogError("aclip: " + (aclip == null) + " " + track.mixPlayable.GetInputCount());
                }
            }
        }


        protected override void OnUpdate(float tick, bool mix)
        {
            if (playable.IsValid())
            {
                if (tick >= aclip.length)
                {
                    if (!anData.loop)
                        tick = aclip.length - 0.01f;
                    else
                        tick = tick % aclip.length;
                }
                playable.SetTime(tick + anData.trim_start);
            }
        }


        protected override void OnExit()
        {
            if (playable.IsValid())
            {
                track.mixPlayable.DisconnectInput(port);
                playable.Destroy();
            }
            base.OnExit();
        }

        protected override void OnDestroy()
        {
            if (playable.IsValid()) playable.Destroy();
            AnimClipData anData = data as AnimClipData;
            XResources.DestroySharedAsset(anData.anim);
            base.OnDestroy();
        }
    }
}
