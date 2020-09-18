using System;
using UnityEngine.Seqence.Data;

namespace UnityEngine.Seqence
{
    [TrackFlag(TrackFlag.SubOnly | TrackFlag.NoClip)]
    [UseParent(typeof(XAnimationTrack))]
    public class XTransformTrack : XTrack, ISharedObject<XTransformTrack>
    {
        private GameObject _target;

        public XTransformTrack next { get; set; }

        public GameObject target
        {
            get
            {
                if (_target == null)
                {
                    if (parent && parent is XBindTrack track)
                    {
                        _target = track.bindObj;
                    }
                }
                return _target;
            }
        }

        private TransformTrackData _data;

        public TransformTrackData Data
        {
            get { return _data; }
        }

        public override AssetType AssetType
        {
            get { return AssetType.Transform; }
        }

        public override XTrack Clone()
        {
            return XSeqenceFactory.GetTrack(data, seqence, parent);
        }


        protected override void OnPostBuild()
        {
            base.OnPostBuild();
            _data = (TransformTrackData)data;
        }

        public bool Sample(float time, out Vector3 pos, out Vector3 rot)
        {
            if (_data?.time == null || _data.time?.Length < 1)
            {
                pos = Vector3.zero;
                rot = Vector3.zero;
                return false;
            }
            int len = _data.time.Length;
            if (time < _data.time[0])
            {
                Vector4 v = _data.pos[0];
                pos = v;
                rot = new Vector3(0, v.w, 0);
                return true;
            }
            else if (time > _data.time[len - 1])
            {
                Vector4 v = _data.pos[len-1];
                pos = v;
                rot = new Vector3(0, v.w, 0);
                return true;
            }
            for (int i = 0; i < len - 1; i++)
            {
                if (time >= _data.time[i] && time <= _data.time[i + 1])
                {
                    float dt = (time - _data.time[i]) / (_data.time[i + 1] - _data.time[i]);
                    Vector4 v = Vector4.Lerp(_data.pos[i], _data.pos[i + 1], dt);
                    pos = v;
                    rot = new Vector3(0, v.w, 0);
                    return true;
                }
            }
            pos = Vector3.zero;
            rot = Vector3.zero;
            return false;
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
                        Vector4 p = pos;
                        p.w = rot.y;
                        _data.pos[i] = p;
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    SeqenceUtil.Add(ref _data.time, t);
                    Vector4 p = pos;
                    p.w = rot.y;
                    SeqenceUtil.Add(ref _data.pos, p);
                }
            }
            else
            {
                _data.time = new[] { t };
                Vector4 p = pos;
                p.w = rot.y;
                _data.pos = new[] { p };
            }
        }

        public bool RmItem(float t)
        {
            if (_data.time != null)
            {
                var time = _data.time;
                for (int i = 0; i < time.Length; i++)
                {
                    if (time[i] == t)
                    {
                        return RmItemAt(i);
                    }
                }
            }
            return false;
        }

        public bool RmItemAt(int i)
        {
            if (_data.time?.Length > i)
            {
                _data.time = SeqenceUtil.Remv(_data.time, i);
                _data.pos = SeqenceUtil.Remv(_data.pos, i);
                return true;
            }
            return false;
        }

        public override void Process(float time, float prev)
        {
            if (!mute)
            {
                if (target != null)
                {
                    Sample(time, out var pos, out var rot);
                    target.transform.localPosition = pos;
                    target.transform.localRotation = Quaternion.Euler(rot);
                }
            }
        }

        public override IClip BuildClip(ClipData data)
        {
            throw new Exception("transform no clip");
        }

        public override void OnDestroy()
        {
            _target = null;
            SharedPool<XTransformTrack>.Return(this);
            base.OnDestroy();
        }

        public void Dispose()
        {
            next = null;
        }
    }
}
