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

        public void Rebind(string prefab)
        {
            if (pat == null || pat != prefab)
            {
                pat = prefab;
                var obj = XResources.LoadGameObject(prefab);
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
            XResources.DestroyGameObject(pat, bindObj);
            base.Dispose();
        }
    }
}
