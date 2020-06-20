using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XBoneFxClip : XClip<XBoneFxTrack>
    {
        private GameObject fx;
        private string path;
        ParticleSystem[] ps;
        uint seed;

        public override string Display
        {
            get { return fx ? fx.name : "fx"; }
        }

        public XBoneFxClip(XBoneFxTrack track, ClipData data) : base(track, data)
        {
            Load((BoneFxClipData) data);
        }

        public void SetFx(GameObject obj)
        {
            fx = obj;
        }

        private void Load(BoneFxClipData data)
        {
            var root = track.root;
            XBindTrack bt = root as XBindTrack;
            seed = data.seed;
            if (bt)
            {
                var go = bt.bindObj;
                if (go != null)
                {
                    var tf = go.transform.Find(data.bone);
                    fx = XResources.LoadGameObject(data.prefab);
                    path = data.prefab;
                    if (fx)
                    {
                        fx.transform.localPosition = data.pos;
                        fx.transform.localRotation = Quaternion.Euler(data.rot);
                        fx.transform.localScale = data.scale;
                        ps = tf.gameObject.GetComponentsInChildren<ParticleSystem>();
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
