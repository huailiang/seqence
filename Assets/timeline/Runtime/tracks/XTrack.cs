using System;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [Flags]
    public enum TrackMode
    {
        Normal = 1,
        Mute = 1 << 1,
        Record = 1 << 2,
        Lock = 1 << 3,
    }


    public abstract class XTrack : IEquatable<XTrack>
    {
        public uint ID;

        protected XTrack[] childs;

        protected IClip[] clips;

        protected XMarker[] marks;

        protected TrackMode mode;

        public XTimeline timeline;

        public XTrack parent { get; set; }

        public XTrack root
        {
            get
            {
                XTrack track = this;
                while (track.parent != null)
                {
                    track = track.parent;
                }
                return track;
            }
        }

        public bool mute
        {
            get { return (mode & TrackMode.Mute) > 0; }
        }

        public bool record
        {
            get { return (mode & TrackMode.Record) > 0; }
        }
        

        protected XTrack(TrackData data)
        {
            ID = XTimeline.IncID;
            mode = TrackMode.Normal;
            if (data != null)
            {
                if (data.clips != null)
                {
                    int len = data.clips.Length;
                    clips = new IClip[len];
                    for (int i = 0; i < len; i++)
                    {
                        clips[i] = BuildClip(data.clips[i]);
                    }
                }

                if (data.marks != null)
                {
                    int len = data.marks.Length;
                    marks = new XMarker[len];
                    for (int i = 0; i < len; i++)
                    {
                        marks[i] = BuildMark(data.marks[i]);
                    }
                }

                if (data.childs != null)
                {
                    int len = data.childs.Length;
                    childs = new XTrack[len];
                    for (int i = 0; i < len; i++)
                    {
                        childs[i] = NewTrack(data.childs[i], timeline);
                        childs[i].parent = this;
                    }
                }
            }
        }


        protected XTrack(XTrack parent, XTrack[] childs, IClip[] clips)
        {
            this.parent = parent;
            this.childs = childs;
            this.clips = clips;
        }

        public static XTrack NewTrack(TrackData data, XTimeline tl)
        {
            XTrack xTrack = null;
            switch (data.type)
            {
                case TrackType.Marker:
                    xTrack = new XMarkerTrack(data);
                    break;
                case TrackType.Animation:
                    xTrack = new XAnimationTrack(data as BindTrackData);
                    break;
                case TrackType.BoneFx:
                    xTrack = new XBoneFxTrack(data);
                    break;
                case TrackType.SceneFx:
                    xTrack = new XSceneFxTrack(data);
                    break;
                default:
                    Debug.LogError("unknown track " + data.type);
                    break;
            }
            if (xTrack) xTrack.timeline = tl;
            return xTrack;
        }

#if UNITY_EDITOR

        public bool locked
        {
            get { return (mode & TrackMode.Lock) > 0; }
        }

        public void AddSub(XTrack track)
        {
            var tmp = new XTrack[childs.Length + 1];
            for (int i = 0; i < childs.Length; i++)
            {
                tmp[i] = childs[i];
            }
            tmp[childs.Length] = track;
            childs = tmp;
        }

        private void Remv()
        {
            if (parent)
            {
                var chs = parent.childs;
                int idx = -1;
                for (int i = 0; i < chs.Length; i++)
                {
                    if (chs[i] == this)
                    {
                        idx = i;
                        break;
                    }
                }
                if (idx >= 0)
                {
                    int len = chs.Length - 1;
                    var tmp = new XTrack[len];
                    for (int i = 0; i < len; i++)
                    {
                        tmp[i] = i < idx ? chs[i] : chs[i + 1];
                    }
                    parent.childs = tmp;
                }
            }
        }
#endif


        protected void Foreach(Action<XTrack> track, Action<IClip> clip)
        {
            ForeachClip(clip);
            ForeachTrack(track);
        }

        public void ForeachMark(Action<XMarker> marker)
        {
            if (marks != null)
            {
                int len = marks.Length;
                for (int i = 0; i < len; i++)
                {
                    marker(marks[i]);
                }
            }
        }

        public void ForeachClip(Action<IClip> clip)
        {
            if (clips != null)
            {
                int len = clips.Length;
                for (int i = 0; i < len; i++)
                {
                    clip(clips[i]);
                }
            }
        }

        public void ForeachTrack(Action<XTrack> track)
        {
            if (childs != null)
            {
                for (int i = 0; i < childs.Length; i++)
                {
                    track(childs[i]);
                }
            }
        }

        public void ForeachHierachyTrack(Action<XTrack> track)
        {
            if (childs != null)
            {
                for (int i = 0; i < childs.Length; i++)
                {
                    track(childs[i]);
                    childs[i].ForeachHierachyTrack(track);
                }
            }
        }

        protected abstract IClip BuildClip(ClipData data);

        protected XMarker BuildMark(MarkData data)
        {
            return new XMarker(this, data);
        }

        public virtual void Process(float time, float prev)
        {
            if (!mute)
            {
                Foreach((track) => track.Process(time, prev), (clip) => clip.Update(time, prev));

                MarkTrigerCheck(time, prev);
            }
        }

        private void MarkTrigerCheck(float time, float prev)
        {
            for (int i = 0; i < marks.Length; i++)
            {
                if (marks[i].time > prev && marks[i].time <= time)
                {
                    marks[i].OnTriger();
                }
                if (marks[i].reverse && marks[i].time >= time && marks[i].time < prev)
                {
                    marks[i].OnTriger();
                }
            }
        }

        public virtual void Dispose()
        {
            Foreach((track) => track.Dispose(), (clip) => clip.Dispose());
            childs = null;
            parent = null;
            ID = 0;
        }


        public override bool Equals(object obj)
        {
            return Equals(obj as XTrack);
        }

        public bool Equals(XTrack other)
        {
            return other != null && ID == other.ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public static bool operator ==(XTrack l, XTrack r)
        {
            if (r == null) return false;
            return l.ID == r.ID;
        }

        public static bool operator !=(XTrack l, XTrack r)
        {
            if (r == null) return true;
            return l.ID != r.ID;
        }

        public static implicit operator bool(XTrack track)
        {
            return track != null;
        }

        public override string ToString()
        {
            return "track " + ID;
        }
    }
}
