using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XBoneFxClip : XClip<XBoneFxTrack>
    {
        public GameObject fx;
        private string path;
        ParticleSystem[] ps;

        public override string Display
        {
            get { return fx ? fx.name : "fx"; }
        }

        public XBoneFxClip(XBoneFxTrack track, ClipData data) : base(track, data)
        {
        }

        public void SetFx(GameObject obj)
        {
            fx = obj;
        }

        public override void OnBind()
        {
            if (fx)
            {
                XResources.DestroyGameObject(path, fx);
            }
            Load((BoneFxClipData) data);
        }

        private void Load(BoneFxClipData data)
        {
            var root = track.root;
            XBindTrack bt = root as XBindTrack;
            var seed = data.seed;
            if (bt)
            {
                var go = bt.bindObj;
                if (go != null && !string.IsNullOrEmpty(data.bone))
                {
                    var tf = go.transform.Find(data.bone);
                    fx = XResources.LoadGameObject(data.prefab);
                    path = data.prefab;
                    if (fx)
                    {
                        fx.transform.parent = tf;
                        fx.transform.localPosition = data.pos;
                        fx.transform.localRotation = Quaternion.Euler(data.rot);
                        fx.transform.localScale = data.scale;
                        ps = fx.GetComponentsInChildren<ParticleSystem>();
                        for (int i = 0; i < ps.Length; i++)
                        {
                            ps[i].randomSeed = seed;
                            ps[i].Stop();
                        }
                    }
                }
            }
        }


        protected override void OnEnter()
        {
            base.OnEnter();
            if (ps != null)
            {
                int len = ps.Length;
                for (int i = 0; i < len; i++)
                {
                    ps[i].Play();
                }
            }
        }

        protected override void OnExit()
        {
            base.OnExit();
            if (ps != null)
            {
                int len = ps.Length;
                for (int i = 0; i < len; i++)
                {
                    ps[i].Stop();
                }
            }
        }

        protected override void OnDestroy()
        {
            if (fx)
            {
                XResources.DestroyGameObject(path, fx);
            }
            ps = null;
            base.OnDestroy();
        }
    }
}
