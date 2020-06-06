using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public abstract class XBindTrack : XTrack
    {
        public GameObject bindObj;

        protected XBindTrack(XTimeline tl, BindTrackData data) : base(tl, data)
        {
            if (!string.IsNullOrEmpty(data.prefab))
            {
                Rebind(data.prefab);
            }
        }

        public void Rebind(string prefab)
        {
            var obj = Resources.Load<GameObject>(prefab);
            bindObj = Object.Instantiate(obj);
            OnBind();
        }

        public void DyncBind(GameObject go)
        {
            bindObj = go;
            OnBind();
        }

        protected virtual void OnBind()
        {
        }
    }
}
