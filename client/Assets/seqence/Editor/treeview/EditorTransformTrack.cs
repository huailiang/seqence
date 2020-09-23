using System;
using UnityEngine;
using UnityEngine.Seqence;
using UnityEngine.Seqence.Data;

namespace UnityEditor.Seqence
{
    [TimelineEditor(typeof(XTransformTrack))]
    public class EditorTransformTrack : RecordTrack
    {
        private TransformTrackData Data;
        private bool folder;

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

        protected override GameObject target
        {
            get
            {
                var p = track?.parent as XBindTrack;
                if (p) return p.bindObj;
                return null;
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
                    float t = Data.time[i];
                    Rect r = RenderRect;
                    DrawKey(t, r, Data.select);
                }
            }
        }


        protected override void ProcessTansfEvent()
        {
            base.ProcessTansfEvent();
            var e = Event.current;
            if (recoding)
            {
                if (e.type == EventType.MouseDown)
                {
                    var t = SeqenceWindow.inst.PiexlToTime(e.mousePosition.x);
                    if (ContainsT(t, out var i))
                    {
                        Data.@select = !Data.@select;
                        e.Use();
                    }
                }
            }
        }


        protected override void OnSelect()
        {
            base.OnSelect();
            if (track.root != null)
            {
                var t = track.root as XBindTrack;
                if (t?.bindObj != null)
                {
                    Selection.activeGameObject = t.bindObj;
                }
            }
        }

        protected override void OnInspectorTrack()
        {
            base.OnInspectorTrack();
            if (track.parent == null) EditorGUILayout.HelpBox("no parent bind", MessageType.Warning);

            GUILayout.Label("time: " + SeqenceWindow.inst.seqence.Time.ToString("f2"));
            bool recd = track.record;
            Vector3 pos = target.transform.localPosition;
            float rot = target.transform.localEulerAngles.y;
            if (recd)
            {
                EditorGUI.BeginChangeCheck();
                using (new GUIColorOverride(Color.red))
                {
                    pos = EditorGUILayout.Vector3Field("pos", pos);
                    rot = EditorGUILayout.FloatField("rotY", rot);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    target.transform.localPosition = pos;
                    var v3 = target.transform.localEulerAngles;
                    v3.y = rot;
                    target.transform.localEulerAngles = v3;
                    AddItem(SeqenceWindow.inst.seqence.Time);
                }
            }
            else
            {
                pos = EditorGUILayout.Vector3Field("pos", pos);
                rot = EditorGUILayout.FloatField("rotY", rot);
            }

            folder = EditorGUILayout.Foldout(folder, "frames");
            if (Data?.time != null)
            {
                if (folder)
                {
                    EditorGUILayout.Space();
                    for (int i = 0; i < Data.time.Length; i++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(16);
                        GUILayout.Label((i + 1) + ". time: " + Data.time[i].ToString("f2"), SeqenceStyle.titleStyle);
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("x", GUI.skin.label, GUILayout.MaxWidth(20)))
                        {
                            (track as XTransformTrack).RmItemAt(i);
                            SeqenceWindow.inst.Repaint();
                            GUIUtility.ExitGUI();
                        }
                        GUILayout.EndHorizontal();

                        pos = Data.pos[i];
                        pos = EditorGUILayout.Vector3Field("pos", pos);
                        rot = Data.pos[i].w;
                        rot = EditorGUILayout.FloatField("rotY", rot);
                        Data.pos[i] = new Vector4(pos.x, pos.y, pos.z, rot);
                        EditorGUILayout.Space();
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("not config time frame", MessageType.Warning);
            }
        }

        protected override void DeleteFrame(Vector2 pos)
        {
            float t = SeqenceWindow.inst.PiexlToTime(pos.x);
            if (ContainsT(t, out var i, 0.4f))
            {
                RmItem(i);
            }
        }

        private bool ContainsT(float t, out int i, float max = 0.1f)
        {
            i = 0;
            var time = Data.time;
            if (time != null)
            {
                for (int j = 0; j < time.Length; j++)
                {
                    if (Mathf.Abs(time[j] - t) < max)
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
            tt.AddItem(t, target.transform.localPosition, target.transform.localEulerAngles);
            SeqenceWindow.inst.Repaint();
        }

        private void RmItem(int i)
        {
            var tt = track as XTransformTrack;
            if (tt.RmItemAt(i)) SeqenceWindow.inst.Repaint();
        }
    }
}
