using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

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

        protected override List<TrackMenuAction> actions
        {
            get
            {
                var types = TypeUtilities.GetRootChilds(typeof(XAnimationTrack));
                List<TrackMenuAction> ret = new List<TrackMenuAction>();
                for (int i = 0; i < types.Count; i++)
                {
                    var str = types[i].ToString();
                    int idx = str.LastIndexOf('.');
                    if (idx >= 0)
                    {
                        str = str.Substring(idx + 1);
                    }
                    var act = new TrackMenuAction()
                    {
                        desc = "Add " + str, on = false, fun = AddSubTrack, arg = types[i]
                    };
                    ret.Add(act);
                }
                return ret;
            }
        }

        private void AddSubTrack(object arg)
        {
            Type type = (Type) arg;
            TrackData data = EditorTrackFactory.CreateData(type);
            var state = TimelineWindow.inst.state;
            var tr = XTrackFactory.Get(data, state.timeline);
            tr.parent = this.track;
            tr.parent.AddSub(tr);
            int idx = TimelineWindow.inst.tree.IndexOfTrack(this.track);
            TimelineWindow.inst.tree.AddTrack(tr, idx + 1);
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
