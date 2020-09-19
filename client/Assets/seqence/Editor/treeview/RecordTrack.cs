using UnityEngine;
using UnityEngine.Seqence;

namespace UnityEditor.Seqence
{
    public abstract class RecordTrack : EditorTrack
    {
        protected static GUIContent s_RecordOn;
        protected static GUIContent s_RecordOff;
        protected static GUIContent s_KeyOn;
        protected static GUIContent s_KeyOff;
        private static AnimationClip animationClip = new AnimationClip();

        protected bool recoding;

        protected abstract GameObject target { get; }

        protected void InitStyle()
        {
            if (s_RecordOn == null)
            {
                s_RecordOn = new GUIContent(SeqenceStyle.autoKey.active.background);
            }
            if (s_RecordOff == null)
            {
                s_RecordOff = new GUIContent(SeqenceStyle.autoKey.normal.background);
            }
            if (s_KeyOn == null)
            {
                s_KeyOn = new GUIContent(SeqenceStyle.keyframe.active.background);
            }
            if (s_KeyOff == null)
            {
                s_KeyOff = new GUIContent(SeqenceStyle.keyframe.normal.background);
            }
        }

        protected override void OnGUIHeader()
        {
            InitStyle();
            bool recd = track.record;
            var content = recd ? s_RecordOn : s_RecordOff;
            if (recd)
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
                if (recd)
                {
                    StopRecd();
                }
                else
                {
                    StartRecd();
                }
                track.SetFlag(TrackMode.Record, !recd);
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
                recoding = true;
                AnimationMode.StartAnimationMode();
                AnimationMode.BeginSampling();
                AnimationMode.SampleAnimationClip(target, animationClip, 0);
            }
        }

        protected virtual void StopRecd()
        {
            recoding = false;
            AnimationMode.EndSampling();
            AnimationMode.StopAnimationMode();
        }
    }
}
