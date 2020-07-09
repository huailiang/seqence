using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XMarker : XTimelineObject
    {
        protected XTrack track { get; set; }

        public MarkData Data { get; set; }

        public MarkType type
        {
            get { return Data.type; }
        }

        protected XTimeline timeline
        {
            get { return track.timeline; }
        }

        public void Initial(XTrack tk, MarkData data)
        {
            track = tk;
            Data = data;
            OnPostBuild();
        }

        protected virtual void OnPostBuild() { }

        public float time
        {
            get { return Data.time; }
            set { Data.time = value; }
        }

        public virtual bool reverse
        {
            get { return Data.reverse; }
            set { Data.reverse = value; }
        }

        public virtual void OnTriger()
        {
        }

        public virtual void OnDestroy()
        {
        }
    }
}
