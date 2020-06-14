using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XLogicClip : XClip<XLogicValueTrack>
    {
        public XLogicClip(XLogicValueTrack track, ClipData data) : base(track, data)
        {
        }

        public override string Display
        {
            get { return "打击点"; }
        }

        protected override void OnEnter()
        {
            base.OnEnter();
        }

        protected override void OnExit()
        {
            base.OnExit();
        }
    }
}
