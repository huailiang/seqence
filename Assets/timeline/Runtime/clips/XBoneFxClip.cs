using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XBoneFxClip : XClip<XBoneFxTrack>
    {
        public GameObject fx;
        private string path;
        ParticleSystem[] ps;
        uint seed;

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
            seed = data.seed;
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
                    }
                }
            }
        }


        protected override void OnUpdate(float time)
        {
            base.OnUpdate(time);
            int len = ps.Length;
            for (int i = 0; i < len; i++)
            {
                ps[i].randomSeed = seed;
                ps[i].Play();
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
