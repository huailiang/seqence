using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public interface IClip
    {
        float start { get; set; }

        float duration { get; set; }

        ClipData data { get; }

        float end { get; }

        string Display { get; }

        void Update(float time, float prev, bool mix);

        void Dispose();

        void OnBind();
    }

    public class XClip<T> : XTimelineObject, IClip where T : XTrack
    {

        private bool enterd = false;

        protected XTimeline timeline
        {
            get { return track.timeline; }
        }

        protected T track { get; }

        public ClipData data { get; }

        protected XClip(T track, ClipData data)
        {
            this.track = track;
            this.data = data;
        }

        public float duration
        {
            get { return data.duration; }
            set { data.duration = value; }
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

        public void Update(float time, float prev, bool mix)
        {
            float tick = time - start;
            if ((time >= start && (time == 0 || prev < start)) || (time <= end && prev > end))
            {
                if (!enterd) OnEnter();
            }
            if (tick >= 0 && time < end)
            {
                if (!enterd) OnEnter();// editor mode can jump when drag time area
                OnUpdate(tick, mix);
            }
            if ((time > end && prev <= end) || (time < start && prev >= start))
            {
                if (enterd) OnExit();
            }
        }

        public virtual void OnBind()
        {
        }

        protected virtual void OnEnter()
        {
            enterd = true;
        }

        protected virtual void OnUpdate(float tick,bool mix)
        {
        }


        protected virtual void OnExit()
        {
            enterd = false;
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
