using UnityEngine.Seqence.Data;

namespace UnityEngine.Seqence
{
    public class XBoneFxClip : XClip<XBoneFxTrack, XBoneFxClip, BoneFxClipData>, ISharedObject<XBoneFxClip>
    {
        public GameObject fx;
        private string path;
        ParticleSystem[] ps;
        private bool restart;

        public override string Display
        {
            get { return fx ? fx.name : "fx"; }
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
            Load((BoneFxClipData)data);
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
                    fx.SetActive(false);
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
            restart = true;
            if (fx) fx.SetActive(true);
        }

        protected override void OnUpdate(float tick, bool mix)
        {
            base.OnUpdate(tick, mix);
            if (ps != null)
            {
                int len = ps.Length;
                for (int i = 0; i < len; i++)
                {
                    ps[i].Simulate(tick, true, restart);
                    restart = false;
                }
            }
        }

        protected override void OnExit()
        {
            base.OnExit();
            if (fx) fx.SetActive(false);
            restart = false;
        }

        public override void OnDestroy()
        {
            if (fx)
            {
                XResources.DestroyGameObject(path, fx);
                fx = null;
            }
            ps = null;
            SharedPool<XBoneFxClip>.Return(this);
            base.OnDestroy();
        }

    }

}