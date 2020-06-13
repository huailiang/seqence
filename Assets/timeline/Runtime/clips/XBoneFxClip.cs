using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XBoneFxClip : XClip<XBoneFxTrack>
    {
        private GameObject fx;
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
                    var obj = Resources.Load<GameObject>(data.prefab);
                    if (obj)
                    {
                        fx = Object.Instantiate<GameObject>(obj);
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
                if (Application.isPlaying)
                {
                    Object.Destroy(fx);
                }
                else
                {
                    Object.DestroyImmediate(fx);
                }
            }
            ps = null;
            base.OnDestroy();
        }
    }
}
