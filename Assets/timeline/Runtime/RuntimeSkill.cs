using UnityEngine.Animations;

namespace UnityEngine.Timeline
{
    public class RuntimeSkill : MonoBehaviour
    {
        public XTimeline timeline;
        public AnimationPlayableOutput playableOutput;
        public AnimationMixerPlayable mixPlayable;


        public void Update()
        {
            if (timeline != null)
            {
                timeline.Update();
            }
            if (Input.GetKey(KeyCode.A))
            {
                timeline = new XTimeline("Assets/timeline.xml");
                timeline.SetPlaying(true);
            }
            if (Input.GetKey(KeyCode.B))
            {
                var track = timeline.SkillHostTrack as XAnimationTrack;
                if (track)
                {
                    playableOutput = track.playableOutput;
                    mixPlayable = track.mixPlayable;
                }
                timeline.BlendTo("Assets/timeline.xml2");
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