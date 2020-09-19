using UnityEngine;
using UnityEngine.Seqence;

namespace UnityEditor.Seqence
{
    public abstract class RecordTrack : EditorTrack
    {
        protected readonly static GUIContent s_RecordOn = new GUIContent(SeqenceStyle.autoKey.active.background);
        protected readonly static GUIContent s_RecordOff = new GUIContent(SeqenceStyle.autoKey.normal.background);
        protected readonly static GUIContent s_KeyOn = new GUIContent(SeqenceStyle.keyframe.active.background);
        protected readonly static GUIContent s_KeyOff = new GUIContent(SeqenceStyle.keyframe.normal.background);
        private static AnimationClip animationClip = new AnimationClip();

        protected bool recoding
        {
            get { return track.record; }
        }

        protected abstract GameObject target { get; }


        public override void OnInit(XSeqenceObject t)
        {
            base.OnInit(t);
            Regist(EventT.Record, OnTrackRecd);
        }

        protected override void OnGUIHeader()
        {
            var content = recoding ? s_RecordOn : s_RecordOff;
            if (recoding)
            {
                float remainder = Time.realtimeSinceStartup % 1;
                SeqenceWindow.inst.Repaint();
                if (remainder < 0.3f)
                {
                    content = SeqenceStyle.empty;
                    addtiveColor = Color.white;
                }
                else
                {
                    addtiveColor = Color.red;
                }
            }
            else
            {
                addtiveColor = Color.white;
            }

            if (GUILayout.Button(content, SeqenceStyle.autoKey, GUILayout.MaxWidth(16)))
            {
                if (recoding)
                {
                    StopRecd();
                }
                else
                {
                    StartRecd();
                }
            }

            if (target && !track.locked)
            {
                ProcessTansfEvent();
            }
        }

        protected override void OnInspectorTrack()
        {
            EditorGUILayout.LabelField("recoding: " + recoding);
            if (target) EditorGUILayout.LabelField("target: " + target.name);
        }

        protected virtual void ProcessTansfEvent()
        {
            var e = Event.current;
            if (recoding)
            {
                if (e.type == EventType.KeyDown)
                {
                    if (e.keyCode == KeyCode.F)
                    {
                        KeyFrame(e.mousePosition);
                    }
                    if (e.keyCode == KeyCode.D || e.keyCode == KeyCode.Delete)
                    {
                        DeleteFrame(e.mousePosition);
                    }
                }
            }
        }

        protected virtual void KeyFrame(Vector2 mouse)
        {
        }

        protected virtual void DeleteFrame(Vector2 mouse)
        {
        }

        protected virtual void StartRecd()
        {
            if (target)
            {
                EventMgr.Send(new EventRecordData());
                track.SetFlag(TrackMode.Record, true);
                AnimationMode.StartAnimationMode();
                AnimationMode.BeginSampling();
                AnimationMode.SampleAnimationClip(target, animationClip, 0);
            }
        }

        protected virtual void StopRecd()
        {
            track.SetFlag(TrackMode.Record, false);
            if (AnimationMode.InAnimationMode())
            {
                AnimationMode.EndSampling();
                AnimationMode.StopAnimationMode();
            }
        }

        protected void OnTrackRecd(EventData d)
        {
            StopRecd();
        }
    }
}
