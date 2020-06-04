using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XSceneFxClip : XClip<XSceneFxTrack>
    {
        public GameObject prefabGameObject;
        private ParticleSystem[] particleSystems;
        private bool restart = false;

        public XSceneFxClip(XSceneFxTrack track, ClipData data) : base(track, data)
        {
            SceneFxClipData fxdata = (SceneFxClipData) data;
            var obj = Resources.Load<GameObject>(fxdata.prefab);
            if (obj != null)
            {
                prefabGameObject = Object.Instantiate(obj);
                var tf = prefabGameObject.transform;
                tf.position = fxdata.pos;
                tf.rotation = Quaternion.Euler(fxdata.rot);
                tf.localScale = fxdata.scale;

                particleSystems = tf.GetComponentsInChildren<ParticleSystem>();
            }
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            restart = true;
        }

        protected override void OnUpdate(float tick)
        {
            if (particleSystems != null)
            {
                int cnt = particleSystems.Length;
                for (int i = 0; i < cnt; i++)
                {
                    particleSystems[i].Simulate(tick, true, restart);
                    restart = false;
                }
            }
        }

        protected override void OnExit()
        {
            base.OnExit();
            restart = false;
            if (prefabGameObject)
            {
                prefabGameObject.SetActive(false);
            }
        }

        protected override void OnDestroy()
        {
            if (prefabGameObject)
            {
                if (Application.isPlaying)
                {
                    Object.Destroy(prefabGameObject);
                }
                else
                {
                    Object.DestroyImmediate(prefabGameObject);
                }
                particleSystems = null;
            }
            base.OnDestroy();
        }
    }
}
