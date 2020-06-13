using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [TimelineEditor(typeof(XTransformTrack))]
    public class EditorTransformTrack : EditorTrack
    {
        static GUIContent s_ArmForRecordContentOn;
        static GUIContent s_ArmForRecordContentOff;

        protected override Color trackColor
        {
            get { return new Color(0.5f, 0.7f, 0.3f); }
        }

        protected override string trackHeader
        {
            get { return "位移" + ID; }
        }

        protected override void OnAddClip(float time)
        {
            throw new Exception("transform no clips");
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
            bool recd = track.record;
            if (GUILayout.Button(recd ? s_ArmForRecordContentOn : s_ArmForRecordContentOff, TimelineStyles.autoKey,
                GUILayout.MaxWidth(16)))
            {
                Debug.Log("start recod mode");
                btrack.SetFlag(TrackMode.Record, !recd);
            }
        }

        protected override void OnGUIContent()
        {
            
        }
    }
}
