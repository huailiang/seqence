using System;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    [TimelineEditor(typeof(XTransformTrack))]
    public class EditorTransformTrack : EditorTrack
    {
        static GUIContent s_RecordOn;
        static GUIContent s_RecordOff;
        static GUIContent s_KeyOn;
        static GUIContent s_KeyOff;
        private TransformTrackData Data;

        protected override Color trackColor
        {
            get { return new Color(0f, 1.0f, 0.8f); }
        }

        protected override bool warn
        {
            get { return track.parent == null || Data.time == null; }
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
            if (s_RecordOn == null)
            {
                s_RecordOn = new GUIContent(TimelineStyles.autoKey.active.background);
            }
            if (s_RecordOff == null)
            {
                s_RecordOff = new GUIContent(TimelineStyles.autoKey.normal.background);
            }
            if (s_KeyOn == null)
            {
                s_KeyOn = new GUIContent(TimelineStyles.keyframe.active.background);
            }
            if (s_KeyOff == null)
            {
                s_KeyOff = new GUIContent(TimelineStyles.keyframe.normal.background);
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
                TimelineWindow.inst.Repaint();
                if (remainder < 0.3f)
                {
                    content = TimelineStyles.empty;
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

            if (GUILayout.Button(content, TimelineStyles.autoKey, GUILayout.MaxWidth(16)))
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
            if (go)
            {
                var e = Event.current;
                if (recoding)
                {
                    if (e.type == EventType.KeyDown)
                    {
                        if (e.keyCode == KeyCode.F)
                        {
                            PrepareOperation(e.mousePosition);
                        }
                    }
                    if (e.type == EventType.MouseDown)
                    {
                        var t = TimelineWindow.inst.PiexlToTime(e.mousePosition.x);
                        if (ContainsT(t, out var i))
                        {
                            Data.@select = !Data.@select;
                            e.Use();
                        }
                    }
                }
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
                    Rect r = RenderRect;
                    r.x = TimelineWindow.inst.TimeToPixel(Data.time[i]);
                    if (TimelineWindow.inst.IsPiexlRange(r.x))
                    {
                        r.width = 20;
                        r.y = RenderRect.y + RenderRect.height / 3;
                        GUIContent gct = Data.@select ? s_KeyOn : s_KeyOff;
                        GUI.Box(r, gct, TimelineStyles.keyframe);
                    }
                }
            }
        }


        protected override void OnInspectorTrack()
        {
            EditorGUILayout.LabelField("recoding: " + recoding);
            if (Data?.time != null)
            {
                if (go) EditorGUILayout.LabelField("target: " + go.name);
                for (int i = 0; i < Data.time.Length; i++)
                {
                    Data.time[i] = EditorGUILayout.FloatField("time", Data.time[i]);
                    Data.pos[i] = EditorGUILayout.Vector3Field("pos", Data.pos[i]);
                    Data.rot[i] = EditorGUILayout.Vector3Field("rot", Data.rot[i]);
                    EditorGUILayout.Space();
                }
            }
        }

        private GameObject go;
        private bool recoding;


        private void PrepareOperation(Vector2 pos)
        {
            float t = TimelineWindow.inst.PiexlToTime(pos.x);
            if (ContainsT(t, out var i))
            {
                if (EditorUtility.DisplayDialog("tip", "Do you want delete item", "ok", "no"))
                {
                    RmItem(i);
                }
                GUIUtility.ExitGUI();
            }
            else
            {
                AddItem(t);
            }
        }

        private bool ContainsT(float t, out int i)
        {
            i = 0;
            var time = Data.time;
            if (time != null)
            {
                for (int j = 0; j < time.Length; j++)
                {
                    if (Mathf.Abs(time[j] - t) < 1f)
                    {
                        i = j;
                        return true;
                    }
                }
            }
            return false;
        }

        private void AddItem(float t)
        {
            var tt = track as XTransformTrack;
            tt.AddItem(t, go.transform.localPosition, go.transform.localEulerAngles);
            TimelineWindow.inst.Repaint();
        }

        private void RmItem(int i)
        {
            var tt = track as XTransformTrack;
            if (tt.RmItemAt(i)) TimelineWindow.inst.Repaint();
        }

        private void StartRecd()
        {
            if (track.parent)
            {
                if (track.parent is XBindTrack bind && bind.bindObj != null)
                {
                    go = bind.bindObj;
                    recoding = true;
                    TimelineWindow.inst.tree?.SetRecordTrack(this);
                }
            }
            else
            {
                EditorUtility.DisplayDialog("warn", "parent track is null or not bind", "ok");
            }
        }

        private void StopRecd()
        {
            recoding = false;
        }
    }
}
