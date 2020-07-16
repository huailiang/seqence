namespace UnityEngine.Timeline
{
    public class RuntimeSkill : MonoBehaviour
    {
        private XSeqence timeline;
        private float time;

        private void Start()
        {
            Application.targetFrameRate = 30;
            string assets = Application.dataPath + "/skill/";
            Debug.Log(assets);
            NativeInterface.Init(Application.targetFrameRate, assets);
        }

        public void Update()
        {
            NativeInterface.Update(Time.deltaTime);
            timeline?.Update();
            if (Input.GetKey(KeyCode.Space))
            {
                Play("sk1001");
            }
        }

        private void OnDestroy()
        {
            timeline?.Dispose();
            NativeInterface.Quit();
        }


        private void Play(string name)
        {
            Debug.Log("play: " + name);
            time = Time.time;
            string path = "Assets/skill/" + name + ".xml";
            if (timeline == null)
            {
                timeline = new XSeqence(path, PlayMode.Skill);
                timeline.SetPlaying(true);
            }
            else
            {
                timeline.BlendTo(path);
            }
        }

#if UNITY_EDITOR

        public void OnDrawGizmos()
        {
            timeline?.ForTrackHierachy(track =>
            {
                if (track is XTransformTrack transformTrack)
                {
                    DrawPath(transformTrack);
                }
            });
        }

        private void DrawPath(XTransformTrack track)
        {
            var ps = track.Data?.pos;
            if (ps != null)
            {
                Gizmos.color = Color.gray;
                for (int i = 0; i < ps.Length - 1; i++)
                {
                    Gizmos.DrawLine(ps[i], ps[i + 1]);
                }
                Gizmos.color = Color.gray;
                Gizmos.DrawSphere(ps[0], 0.04f);
            }
        }

#endif
    }

}
