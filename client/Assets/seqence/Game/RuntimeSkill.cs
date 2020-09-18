namespace UnityEngine.Seqence
{
    public class RuntimeSkill : MonoBehaviour
    {

        private void Start()
        {
            Application.targetFrameRate = 30;
            string assets = Application.dataPath + "/skill/";
            Debug.Log(assets);
            NativeInterface.Init(Application.targetFrameRate, assets);
        }

        public void Update()
        {
            float delta = Time.deltaTime;
            NativeInterface.Update(delta);
            EntityMgr.Instance.Update(delta);
        }

        private void OnDestroy()
        {
            NativeInterface.Quit();
        }

    }
}