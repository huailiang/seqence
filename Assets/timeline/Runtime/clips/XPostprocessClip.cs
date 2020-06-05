using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XPostprocessClip : XClip<XPostprocessTrack>
    {
        public XPostprocessClip(XPostprocessTrack track, ClipData data)
            : base(track, data)
        {
        }
    }
}