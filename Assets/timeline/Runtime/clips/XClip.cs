using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public interface IClip
    {
        float start { get; set; }

        float duration { get; }

        float end { get; }

        string Display { get; }

        void Update(float time, float prev);

        void Dispose();
    }

    public class XClip<T> : IClip where T : XTrack
    {
        protected XTimeline timeline
        {
            get { return track.timeline; }
        }

        protected T track { get; set; }

        protected ClipData data { get; set; }

        public XClip(T track, ClipData data)
        {
            this.track = track;
            this.data = data;
        }

        public float duration
        {
            get { return data.duration; }
        }

        public float start
        {
            get { return data.start; }
            set { data.start = value; }
        }

        public float end
        {
            get { return start + duration; }
        }

        public virtual string Display
        {
            get { return string.Empty; }
        }

        public void Dispose()
        {
            OnDestroy();
        }

        public void Update(float time, float prev)
        {
            float tick = time - start;
            if ((time >= start && prev < start) || (time <= end && prev > end))
            {
                OnEnter();
            }
            if ((time > end && prev <= time) || (time < start || prev >= start))
            {
                OnExit();
            }
            if (tick >= 0)
            {
                OnUpdate(tick);
            }
        }

        protected virtual void OnEnter()
        {
        }

        protected virtual void OnUpdate(float tick)
        {
        }


        protected virtual void OnExit()
        {
        }

        protected virtual void OnDestroy()
        {
        }


        public static implicit operator bool(XClip<T> clip)
        {
            return clip != null;
        }
    }
}
