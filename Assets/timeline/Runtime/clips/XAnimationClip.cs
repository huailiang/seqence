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
        private bool loop = false;
        private float clipLen = 0;

        public override string Display
        {
            get { return aclip != null ? aclip.name + " " + port : " anim:" + port; }
        }

        public XAnimationClip(XAnimationTrack track, ClipData data) : base(track, data)
        {
            AnimClipData anData = data as AnimClipData;
            aclip = XResources.LoadSharedAsset<AnimationClip>(anData.anim);
            loop = anData.loop;
            clipLen = aclip?.length ?? 0;
        }


        protected override void OnEnter()
        {
            base.OnEnter();
            if (track.mixPlayable.IsValid())
            {
                int cnt = track.mixPlayable.GetInputCount();
                track.mixPlayable.SetInputCount(++cnt);
                playable = AnimationClipPlayable.Create(timeline.graph, aclip);
                timeline.graph.Connect(playable, 0, track.mixPlayable, port);
                track.mixPlayable.SetInputWeight(port, 1);
            }
        }


        protected override void OnUpdate(float tick)
        {
            if (timeline.isRunning)
            {
                playable.SetTime(tick);
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
            if(playable.IsValid()) playable.Destroy();
            AnimClipData anData = data as AnimClipData;
            XResources.DestroySharedAsset(anData.anim);
            base.OnDestroy();
        }
    }
}
