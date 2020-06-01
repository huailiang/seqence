using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XAnimationClip : XClip<XAnimationTrack>
    {
        AnimationClipPlayable playable;

        public int port = 0;


        public XAnimationClip(XAnimationTrack track, ClipData data) : base(track, data)
        {
            AnimClipData anData = data as AnimClipData;
            var clip = Resources.Load<AnimationClip>(anData.anim);
            playable = AnimationClipPlayable.Create(timeline.graph, clip);
            track.playableOutput.SetSourcePlayable(playable, 0);
        }


        protected override void OnUpdate(float tick)
        {
            playable.SetTime(tick);
        }
    }
}
