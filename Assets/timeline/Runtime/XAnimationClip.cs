using UnityEngine;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{

    public class XAnimationClip : XClip<XAnimationTrack>
    {
        
        public XAnimationClip(XAnimationTrack track, ClipData data) :
         base(track, data)
        {
        }


        protected override void OnUpdate(float tick)
        {

        }

    }


}