using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XPostprocessClip : XClip<XPostprocessTrack>
    {
        public override string Display
        {
            get { return "post process"; }
        }

        public XPostprocessClip(XPostprocessTrack track, ClipData data) : base(track, data)
        {
        }
    }
}
