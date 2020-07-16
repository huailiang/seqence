using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public abstract class XBindTrack : XTrack
    {
        public GameObject bindObj;
        private string pat;

        protected override void OnPostBuild()
        {
            base.OnPostBuild();
            BindTrackData bind = data as BindTrackData;
            if (!string.IsNullOrEmpty(bind.prefab))
            {
                Rebind(bind.prefab);
            }
        }

        public void Load()
        {
            if (data is BindTrackData bd && !string.IsNullOrEmpty(bd.prefab))
            {
                Rebind(bd.prefab);
            }
        }

        private void Rebind(string prefab)
        {
            if (!string.IsNullOrEmpty(prefab) && bindObj == null)
            {
                pat = prefab;
                GameObject obj = null;
                if (seqence.IsHostTrack(this) && seqence.hostAnimator != null)
                {
                    obj = seqence.hostAnimator.gameObject;
                }
                else
                {
                    obj = XResources.LoadGameObject(prefab);
                }
                if (obj)
                {
                    bindObj = obj;
                    seqence.BindGo(bindObj);
                    (data as BindTrackData).prefab = prefab;
                    //childs & self
                    ForeachHierachyTrack(x => x.OnBind());
                }
            }
        }


        public override void OnDestroy()
        {
            if (!seqence.IsHostTrack(this))
            {
                XResources.DestroyGameObject(pat, bindObj);
            }
            base.OnDestroy();
        }
    }
}
