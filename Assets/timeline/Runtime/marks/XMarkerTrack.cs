using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XMarkerTrack : XTrack
    {
        
        
        public XMarkerTrack(TrackData data) : base(data)
        {
        }

        protected override IClip BuildClip(ClipData data)
        {
            throw new System.NotImplementedException();
        }

       
    }
}