using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [MarkUsage(AssetType.Marker)]
    public class XSlowMarker : XMarker
    {
        private SlowMarkData _data;

        public float slow
        {
            get { return _data.slowRate; }
            set { _data.slowRate = value; }
        }

        public XSlowMarker(XTrack track, MarkData data) : base(track, data)
        {
            _data = (SlowMarkData) data;
        }

        public override void OnTriger()
        {
            base.OnTriger();
            timeline.slow = slow;
        }
    }
}
