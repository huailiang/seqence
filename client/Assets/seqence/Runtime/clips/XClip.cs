using UnityEngine.Seqence.Data;

namespace UnityEngine.Seqence
{
    public interface IClip
    {
        float start { get; set; }

        float duration { get; set; }

        ClipData Data { get; }

        float end { get; }

        string Display { get; }

        bool Update(float time, float prev, bool mix);

        void OnDestroy();

        void OnBind();
    }

    public class XClip<T, C, D> : XSeqenceObject, IClip where T : XTrack where C : XClip<T, C, D> where D : ClipData
    {
        private bool enterd = false;

        public C next { get; set; }

        protected XSeqence seqence
        {
            get { return track.seqence; }
        }

        public T track { get; set; }

        public D data { get; set; }

        public ClipData Data
        {
            get { return data; }
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

        public bool Update(float time, float prev, bool mix)
        {
            float tick = time - start;
            bool rst = false;
            if ((time >= start && (time == 0 || prev < start)) || (time <= end && prev > end))
            {
                if (!enterd) OnEnter();
            }
            if (tick >= 0 && time < end)
            {
                if (!enterd) OnEnter(); // editor mode can jump when drag time area
                OnUpdate(tick, mix);
                rst = true;
            }
            if ((time > end && prev <= end) || (time < start && prev >= start))
            {
                if (enterd) OnExit();
            }
            return rst;
        }

        public virtual void OnBind()
        {
        }

        protected virtual void OnEnter()
        {
            enterd = true;
        }

        protected virtual void OnUpdate(float tick, bool mix)
        {
        }


        protected virtual void OnExit()
        {
            enterd = false;
        }

        public virtual void OnDestroy()
        {
            enterd = false;
            data = null;
            track = null;
        }
        
        public static implicit operator bool(XClip<T, C, D> clip)
        {
            return clip != null;
        }

        public void Dispose()
        {
            next = null;
        }
    }
}
