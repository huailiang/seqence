using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [MarkUsage(TrackType.Marker)]
    public class XSlowMarker : XMarker
    {
        private SlowMarkData _data;

        public float slow
        {
            get { return _data.slowRate; }
            set { _data.slowRate = value; }
        }

        public XSlowMarker(XTrack track, SlowMarkData data) : base(track, data)
        {
            _data = data;
        }

        public override void OnTriger()
        {
            base.OnTriger();
            timeline.slow = slow;
        }
    }
}
