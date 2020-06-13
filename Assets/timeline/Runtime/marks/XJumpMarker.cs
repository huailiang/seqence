using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [MarkUsage(TrackType.Marker)]
    public class XJumpMarker : XMarker
    {
        private JumpMarkData _data;

        public float jump
        {
            get { return _data.jump; }
            set { _data.jump = value; }
        }

        public XJumpMarker(XTrack track, JumpMarkData data) : base(track, data)
        {
            _data = data;
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
