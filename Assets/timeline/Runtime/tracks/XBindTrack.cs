using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public abstract class XBindTrack : XTrack
    {
        public GameObject bindObj;
        private string pat;

        protected XBindTrack(XTimeline tl, BindTrackData data) : base(tl, data)
        {
            if (!string.IsNullOrEmpty(data.prefab))
            {
                Rebind(data.prefab);
            }
        }

        public void Load()
        {
            if (data is BindTrackData bd && !string.IsNullOrEmpty(bd.prefab))
            {
                Rebind(bd.prefab);
            }
        }

        public void Rebind(string prefab)
        {
            if (!string.IsNullOrEmpty(prefab) && bindObj == null)
            {
                pat = prefab;
                GameObject obj = null;
                if (timeline.IsHostTrack(this))
                {
                    obj = timeline.blendPlayableOutput.GetTarget().gameObject;
                }
                else
                {
                    obj = XResources.LoadGameObject(prefab);
                }
                if (obj)
                {
                    bindObj = obj;
                    timeline.BindGo(bindObj);
                    (data as BindTrackData).prefab = prefab;
                    //childs & self
                    ForeachHierachyTrack(x => x.OnBind());
                }
            }
        }


        public override void Dispose()
        {
            if (!timeline.IsHostTrack(this))
            {
                XResources.DestroyGameObject(pat, bindObj);
            }
            base.Dispose();
        }
    }
}
