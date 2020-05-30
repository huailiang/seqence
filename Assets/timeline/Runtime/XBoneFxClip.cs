using UnityEngine;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XBoneFxClip : XClip<XBoneFxTrack>
    {

        ParticleSystem[] ps;
        uint seed;

        public XBoneFxClip(XBoneFxTrack track, ClipData data) :
            base(track, data)
        {
            Load((BoneFxClipData)data);
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
                    var fx = Resources.Load<GameObject>(data.prefab);
                    fx = GameObject.Instantiate<GameObject>(fx);
                    fx.transform.localPosition = data.pos;
                    fx.transform.localRotation = Quaternion.Euler(data.rot);
                    fx.transform.localScale = data.scale;
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

    }

}