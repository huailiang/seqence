using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public abstract class XBindTrack : XTrack
    {
        public GameObject bindObj;
        private bool innerLoad;

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
            innerLoad = true;
            OnBind();
        }

        public void DyncBind(GameObject go)
        {
            bindObj = go;
            innerLoad = false;
            OnBind();
        }

        public override void Dispose()
        {
            if (innerLoad)
            {
                if (Application.isPlaying)
                {
                    Object.Destroy(bindObj);
                }
                else
                {
                    Object.DestroyImmediate(bindObj);
                }
            }
            base.Dispose();
        }

        protected virtual void OnBind()
        {
        }
        
    }
}
