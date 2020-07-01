#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.Timeline
{
    public class RuntimeTimeline : MonoBehaviour
    {
        public string path;

        public XTimeline timeline;
        private bool play;
        private float time;

        public void Start()
        {

            if (Application.isPlaying)
            {
                timeline = new XTimeline(path);
                timeline.mode = TimelinePlayMode.RealRunning;
            }
            play = true;
            time = 0;
        }


        public void Update()
        {
            if (play)
            {
                time += Time.deltaTime;
                timeline?.Process(time);
            }
        }

        public void OnDrawGizmos()
        {
#if UNITY_EDITOR
            timeline?.ForTrackHierachy(track=>
            {
                if(track is XTransformTrack transformTrack)
                {
                    DrawPath(transformTrack);
                }
            });
#endif
        }

#if UNITY_EDITOR
        private void DrawPath(XTransformTrack track)
        {
            var ps = track.Data?.pos;
            if (ps != null)
            {
                Gizmos.color = Color.gray;
                for (int i = 0; i < ps.Length-1; i++)
                {
                    Gizmos.DrawLine(ps[i], ps[i + 1]);
                }
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(ps[0], 0.1f);
            }
        }
#endif
    }
}