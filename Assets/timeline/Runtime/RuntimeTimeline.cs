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

        public void Start()
        {
            if (Application.isPlaying)
            {
                timeline = new XTimeline(path);
                timeline.SetPlaying(true);
            }
            play = true;
        }


        public void Update()
        {
            if (play)
            {
                timeline?.Update();
            }
        }


        private void OnDestroy()
        {
            timeline?.Dispose();
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