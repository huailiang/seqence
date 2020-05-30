using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{

    public class XMarker
    {
        protected XTrack track { get; set; }

        protected MarkData data { get; set; }

        protected XTimeline timeline
        {
            get { return track.timeline; }
        }


        public XMarker(XTrack track, MarkData data)
        {
            this.track = track;
            this.data = data;
        }


        public float time
        {
            get { return data.time; }
        }

        public virtual bool reverse
        {
            get { return true; }
        }

        public virtual void OnTriger() { }

    }

}
