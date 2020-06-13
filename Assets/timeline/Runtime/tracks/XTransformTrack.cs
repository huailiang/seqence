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
        }

        protected override IClip BuildClip(ClipData data)
        {
            throw new System.NotImplementedException();
        }
    }
}
