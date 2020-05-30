using UnityEngine;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    
    public abstract class XBindTrack : XTrack
    {

        public GameObject bindObj;

        public XBindTrack(BindTrackData data) :
            base(data)
        {
            var obj = Resources.Load<GameObject>(data.prefab);
            bindObj = GameObject.Instantiate<GameObject>(obj);
        }

    }

}