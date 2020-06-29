using System;
using System.Collections.Generic;
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


    public abstract class XTrack : XTimelineObject
    {
        public readonly uint ID;

        public XTrack[] childs;

        public IClip[] clips;

        public XMarker[] marks;

        public List<IMixClip> mixs;

        protected TrackMode mode;

        public XTimeline timeline;

        public TrackData data;

        public XTrack parent { get; set; }

        public virtual bool cloneable
        {
            get { return true; }
        }

        public abstract AssetType AssetType { get; }

        public abstract XTrack Clone();

        public bool hasChilds
        {
            get { return childs != null && childs.Length > 0; }
        }

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
            get { return GetFlag(TrackMode.Mute); }
        }

        public bool parentMute
        {
            get { return parent ? parent.mute : mute; }
        }

        public bool record
        {
            get { return GetFlag(TrackMode.Record); }
        }

        public bool locked
        {
            get { return GetFlag(TrackMode.Lock); }
        }

        public bool lockedHirachy
        {
            get
            {
                if (!locked)
                {
                    return parentLocked;
                }
                return true;
            }
        }

        public bool parentLocked
        {
            get { return parent ? parent.locked : locked; }
        }

        protected XTrack(XTimeline tl, TrackData data)
        {
            timeline = tl;
            this.data = data;
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
                        marks[i] = XTimelineFactory.GetMarker(this, data.marks[i]);
                    }
                }

                if (data.childs != null)
                {
                    int len = data.childs.Length;
                    childs = new XTrack[len];
                    for (int i = 0; i < len; i++)
                    {
                        childs[i] = XTimelineFactory.GetTrack(data.childs[i], timeline, this);
                    }
                }
            }
        }

        public bool GetFlag(TrackMode mode)
        {
            return (this.mode & mode) > 0;
        }

        public void SetFlag(TrackMode mode, bool flag)
        {
            if (flag)
            {
                this.mode |= mode;
            }
            else
            {
                this.mode &= ~(mode);
            }
        }

        public bool IsChild(XTrack p, bool gradsonContains)
        {
            XTrack tmp = this;
            if (gradsonContains)
            {
                while (tmp)
                {
                    if (tmp.parent != null)
                    {
                        if (tmp.parent.Equals(p))
                        {
                            return true;
                        }
                        else
                        {
                            tmp = tmp.parent;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                return p != null && this.parent.Equals(p);
            }
            return false;
        }

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
            track(this);
        }

        protected abstract IClip BuildClip(ClipData data);

        public virtual void OnPostBuild()
        {
        }

        protected void AddMix(IMixClip mix)
        {
            if (mixs == null)
            {
                mixs = new List<IMixClip>();
            }
            if (!mixs.Contains(mix))
            {
                mixs.Add(mix);
            }
        }

        public virtual void OnBind()
        {
            ForeachClip(x => x.OnBind());
        }

        public virtual void Process(float time, float prev)
        {
            ForeachTrack(track => track.Process(time, prev));
            if (!mute)
            {
                bool mix = MixTriger(time, out var mixClip);
                ForeachClip(clip => clip.Update(time, prev, mix));
                MarkTriger(time, prev);
                if (mix) OnMixer(time, mixClip);
            }
        }

        public void RebuildMix()
        {
            mixs?.Clear();
            if (clips != null)
            {
                float tmp = clips[0].end;
                for (int i = 1; i < clips.Length; i++)
                {
                    if (clips[i].start > tmp)
                    {
                        float start = tmp;
                        float duration = clips[i].start - tmp;
                        var mix = new XMixClip<XAnimationTrack>(start, duration, clips[i - 1], clips[i]);
                        AddMix(mix);
                    }
                    tmp = clips[i].end;
                }
            }
        }

        protected virtual void OnMixer(float time, IMixClip mix)
        {
        }

        private bool MixTriger(float time, out IMixClip mixClip)
        {
            if (mixs != null)
            {
                int cnt = mixs.Count;
                for (int i = 0; i < cnt; i++)
                {
                    if (mixs[i].IsIn(time))
                    {
                        mixClip = mixs[i];
                        return true;
                    }
                }
            }
            mixClip = null;
            return false;
        }

        private void MarkTriger(float time, float prev)
        {
            if (marks != null)
            {
                for (int i = 0; i < marks.Length; i++)
                {
                    var mark = marks[i];
                    if (mark.time > prev && mark.time <= time)
                    {
                        mark.OnTriger();
                    }
                    if (mark.reverse && mark.time >= time && mark.time < prev)
                    {
                        mark.OnTriger();
                    }
                }
            }
        }

        public virtual void Dispose()
        {
            Foreach(track => track.Dispose(), clip => clip.Dispose());
            ForeachMark(mark => mark.Dispose());
            childs = null;
            parent = null;
            if (mixs != null)
            {
                mixs.Clear();
                mixs = null;
            }
            marks = null;
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
            return ((ID << 4) + 375).GetHashCode();
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
