using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [MarkUsage(TrackType.Marker)]
    public class XJumpMarker : XMarker
    {
        private float jump;

        public XJumpMarker(XTrack track, JumpMarkData data) : base(track, data)
        {
            jump = data.jump;
        }


        public override void OnTriger()
        {
            base.OnTriger();
            if (jump != timeline.Time)
            {
                timeline.ProcessImmediately(jump);
            }
        }
    }
}
