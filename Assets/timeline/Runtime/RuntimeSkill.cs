namespace UnityEngine.Timeline
{
    public class RuntimeSkill : MonoBehaviour
    {
        private XTimeline timeline;
        private float time;
        private bool playing;

        private void Start()
        {
            Application.targetFrameRate = 30;
        }

        public void Update()
        {
            timeline?.Update();
            if (Input.GetKey(KeyCode.Space))
            {
                playing = true;
                Play("sk1001");
            }
            if (playing && Time.time - time > 0.8f)
            {
                Play("sk1002");
                playing = false;
            }
        }

        private void OnDestroy()
        {
            timeline?.Dispose();
        }


        private void Play(string name)
        {
            Debug.Log("play: " + name);
            time = Time.time;
            string path = "Assets/skill/" + name + ".xml";
            if (timeline == null)
            {
                timeline = new XTimeline(path, PlayMode.Skill);
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
