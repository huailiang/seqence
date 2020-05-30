using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XSlowMarker : XMarker
    {

        private float slow;

        public XSlowMarker(XTrack track, SlowMarkData data) : base(track, data)
        {
            slow = data.slowRate;
        }

        public override void OnTriger()
        {
            base.OnTriger();
        }

    }

}