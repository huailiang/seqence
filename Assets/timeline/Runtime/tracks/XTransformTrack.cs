using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [TrackFlag(TrackFlag.SubOnly | TrackFlag.NoClip)]
    [UseParent(typeof(XAnimationTrack))]
    public class XTransformTrack : XTrack
    {
        public GameObject target
        {
            get
            {
                if (parent && parent is XBindTrack track)
                {
                    return track.bindObj;
                }
                return null;
            }
        }

        private TransformTrackData _data;

        public TransformTrackData Data
        {
            get { return _data; }
        }

        public override TrackType trackType
        {
            get { return TrackType.Transform; }
        }

        public override XTrack Clone()
        {
            throw new System.NotImplementedException();
        }

        public XTransformTrack(XTimeline tl, TrackData data) : base(tl, data)
        {
            _data = (TransformTrackData) data;
        }


        public void AddItem(float t, Vector3 pos, Vector3 rot)
        {
            if (_data.time != null)
            {
                var time = _data.time;
                bool find = false;
                for (int i = 0; i < time.Length; i++)
                {
                    if (time[i] == t)
                    {
                        _data.pos[i] = pos;
                        _data.rot[i] = rot;
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    TimelineUtil.Add(ref _data.time, t);
                    TimelineUtil.Add(ref _data.pos, pos);
                    TimelineUtil.Add(ref _data.rot, rot);
                }
            }
        }

        public void RmItem(float t)
        {
            if (_data.time != null)
            {
                var time = _data.time;
                for (int i = 0; i < time.Length; i++)
                {
                    if (time[i] == t)
                    {
                        _data.time = TimelineUtil.Remv(_data.time, i);
                        _data.pos = TimelineUtil.Remv(_data.pos, i);
                        _data.rot = TimelineUtil.Remv(_data.rot, i);
                        break;
                    }
                }
            }
        }

        protected override IClip BuildClip(ClipData data)
        {
            throw new System.NotImplementedException();
        }
    }
}
