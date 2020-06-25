using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XSceneFxClip : XClip<XSceneFxTrack>
    {
        public GameObject prefabGameObject;
        private string path;
        private ParticleSystem[] particleSystems;
        private bool restart;

        public override string Display
        {
            get { return prefabGameObject != null ? prefabGameObject.name : ""; }
        }

        public XSceneFxClip(XSceneFxTrack track, ClipData data) : base(track, data)
        {
            SceneFxClipData fxdata = (SceneFxClipData) data;
            Load(fxdata.prefab, fxdata.pos, fxdata.rot, fxdata.scale);
        }

        public void SetReference(GameObject refObj)
        {
            prefabGameObject = refObj;
        }

        public void Load(string path)
        {
            Load(path, Vector3.zero, Vector3.zero, Vector3.one);
        }

        public void Load(string path, Vector3 pos, Vector3 rot, Vector3 scale)
        {
            this.path = path;
            prefabGameObject = XResources.LoadGameObject(path);
            if (prefabGameObject != null)
            {
                timeline.BindGo(prefabGameObject);
                var tf = prefabGameObject.transform;
                tf.position = pos;
                tf.rotation = Quaternion.Euler(rot);
                tf.localScale = scale;
                particleSystems = tf.GetComponentsInChildren<ParticleSystem>();
            }
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            restart = true;
            if (prefabGameObject)
            {
                prefabGameObject.SetActive(true);
            }
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
                XResources.DestroyGameObject(path, prefabGameObject);
                particleSystems = null;
            }
            base.OnDestroy();
        }
    }
}
