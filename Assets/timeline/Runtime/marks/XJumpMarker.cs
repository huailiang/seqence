using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XJumpMarker : XMarker
    {

        private float jump;

        public XJumpMarker(XTrack track, JumpMarkData data) :
            base(track, data)
        {
            jump = data.jump;
        }


        public override void OnTriger()
        {
            base.OnTriger();
            timeline.Process(jump);
        }

    }


}