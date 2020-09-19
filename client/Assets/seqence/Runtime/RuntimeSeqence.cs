namespace UnityEngine.Seqence
{
    public class RuntimeSeqence : MonoBehaviour
    {
        public int a;
        public Color c;
        public string path;
        public XSeqence seqence;
        private bool play;

        public void Start()
        {
            Application.targetFrameRate = 30;
            if (Application.isPlaying)
            {
                seqence = new XSeqence(path, PlayMode.Plot);
                seqence.SetPlaying(true);
            }
            play = true;
        }

        public void Update()
        {
            if (play)
            {
                seqence?.Update();
            }
        }

        private void OnDestroy()
        {
            seqence?.Dispose();
        }

#if UNITY_EDITOR

        public void OnDrawGizmos()
        {
            seqence?.ForTrackHierachy(track =>
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
