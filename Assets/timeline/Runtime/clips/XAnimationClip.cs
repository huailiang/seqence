using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XAnimationClip : XClip<XAnimationTrack>
    {
        public AnimationClipPlayable playable;
        public AnimationClip aclip;
        private AnimClipData anData;
        public int port = 0;

        public override string Display
        {
            get { return aclip != null ? aclip.name + " " + port : " anim" + port; }
        }

        public XAnimationClip(XAnimationTrack track, ClipData data) : base(track, data)
        {
            anData = data as AnimClipData;
            aclip = XResources.LoadSharedAsset<AnimationClip>(anData.anim);
            playable = AnimationClipPlayable.Create(XTimeline.graph, aclip);
        }

        public override void OnBind()
        {
            base.OnBind();
            track.mixPlayable.AddInput(playable, 0, 1);
        }


        protected override void OnUpdate(float tick, bool mix)
        {
            if (playable.IsValid())
            {
                float offset = anData.trim_start;
                tick = tick + offset;
                if (tick >= aclip.length)
                {
                    if (!anData.loop)
                        tick = aclip.length - 0.01f;
                    else
                    {
                        tick = tick % aclip.length;
                    }
                }
                if (playable.IsValid())
                    playable.SetTime(tick);
                else
                    Debug.Log("playable is invalid");
            }
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
