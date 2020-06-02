using UnityEngine;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public abstract class XBindTrack : XTrack
    {
        public GameObject bindObj;

        protected XBindTrack(XTimeline tl, BindTrackData data) 
            : base(tl, data)
        {
            var obj = Resources.Load<GameObject>(data.prefab);
            bindObj = Object.Instantiate<GameObject>(obj);
        }
    }
}
