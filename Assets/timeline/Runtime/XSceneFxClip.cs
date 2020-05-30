using UnityEngine;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XSceneFxClip : XClip<XSceneFxTrack>
    {
        
        public XSceneFxClip(XSceneFxTrack track, ClipData data) :
           base(track, data)
        {
        }

        protected override void OnAwake()
        {
            base.OnAwake();
        }

        protected override void OnEnter()
        {
            base.OnEnter();
        }

        protected override void OnUpdate(float tick)
        {
        }

        protected override void OnExit()
        {
            base.OnExit();
        }
    }

}