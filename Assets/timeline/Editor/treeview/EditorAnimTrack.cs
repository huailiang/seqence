using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public class EditorAnimTrack : EditorTrack
    {
        static GUIContent s_ArmForRecordContentOn;
        static GUIContent s_ArmForRecordContentOff;

        protected override Color trackColor
        {
            get { return Color.yellow; }
        }

        private void InitStyle()
        {
            if (s_ArmForRecordContentOn == null)
            {
                s_ArmForRecordContentOn = new GUIContent(TimelineStyles.autoKey.active.background);
            }
            if (s_ArmForRecordContentOff == null)
            {
                s_ArmForRecordContentOff = new GUIContent(TimelineStyles.autoKey.normal.background);
            }
        }

        protected override void OnGUIHeader()
        {
            InitStyle();
            XBindTrack btrack = track as XBindTrack;
            if (btrack)
            {
#pragma warning disable 618
                btrack.bindObj =
                    (GameObject) EditorGUILayout.ObjectField(btrack.bindObj, typeof(Animator), GUILayout.MaxWidth(80));
#pragma warning restore 618
            }
            if (GUILayout.Button("", TimelineStyles.clips))
            {
                Debug.Log("start recod mode");
                track.SetFlag(TrackMode.Record, !track.record);
            }
            bool recd = track.record;
            if (GUILayout.Button(recd ? s_ArmForRecordContentOn : s_ArmForRecordContentOff, TimelineStyles.autoKey))
            {
                Debug.Log("show record clip");
                btrack.SetFlag(TrackMode.Record, !recd);
            }
        }

        protected override void OnGUIContent()
        {
        }
    }
}
