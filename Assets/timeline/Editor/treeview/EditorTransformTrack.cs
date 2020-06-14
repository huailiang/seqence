using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    [TimelineEditor(typeof(XTransformTrack))]
    public class EditorTransformTrack : EditorTrack
    {
        static GUIContent s_ArmForRecordContentOn;
        static GUIContent s_ArmForRecordContentOff;
        private TransformTrackData Data;

        protected override Color trackColor
        {
            get { return new Color(0.5f, 0.7f, 0.3f); }
        }

        protected override string trackHeader
        {
            get { return "位移" + ID; }
        }

        protected override List<TrackMenuAction> actions
        {
            get
            {
                List<TrackMenuAction> retl = new List<TrackMenuAction>();
                TrackMenuAction action = new TrackMenuAction();
                action.desc = "Add Item";
                action.fun = OnAdditem;
                action.arg = 0;
                retl.Add(action);
                return retl;
            }
        }

        private void OnAdditem(object arg)
        {
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

            bool recd = track.record;
            var content = recd ? s_ArmForRecordContentOn : s_ArmForRecordContentOff;
            if (GUILayout.Button(content, TimelineStyles.autoKey, GUILayout.MaxWidth(16)))
            {
                Debug.Log("start recod mode");
                track.SetFlag(TrackMode.Record, !recd);
            }
        }

        protected override void OnGUIContent()
        {
            if (Data == null)
            {
                var tt = (track as XTransformTrack);
                Data = tt?.Data;
            }
            if (Data?.time != null)
            {
                for (int i = 0; i < Data.time.Length; i++)
                {
                    Rect r = rect;
                    r.x = TimelineWindow.inst.TimeToPixel(Data.time[i]);
                    r.width = 16;
                    GUI.Box(r, "", TimelineStyles.keyframe);
                }
            }
        }
    }
}
