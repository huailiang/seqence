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
                    Rect r = RenderRect;
                    r.x = SeqenceWindow.inst.TimeToPixel(Data.time[i]);
                    if (SeqenceWindow.inst.IsPiexlRange(r.x))
                    {
                        r.width = 20;
                        r.y = RenderRect.y + RenderRect.height / 3;
                        GUIContent gct = Data.select ? s_KeyOn : s_KeyOff;
                        GUI.Box(r, gct, SeqenceStyle.keyframe);
                    }
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
            if (track.parent == null)
                EditorGUILayout.HelpBox("no parent bind", MessageType.Warning);
            if (Data?.time != null)
            {
                for (int i = 0; i < Data.time.Length; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("time: " + Data.time[i]);
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("x", GUI.skin.label, GUILayout.MaxWidth(20)))
                    {
                        (track as XTransformTrack).RmItemAt(i);
                        SeqenceWindow.inst.Repaint();
                        GUIUtility.ExitGUI();
                    }
                    EditorGUILayout.EndHorizontal();
                    Vector3 pos = Data.pos[i];
                    pos = EditorGUILayout.Vector3Field("pos", pos);
                    float rot = Data.pos[i].w;
                    rot = EditorGUILayout.FloatField("rotY", rot);
                    Data.pos[i] = new Vector4(pos.x, pos.y, pos.z, rot);
                    EditorGUILayout.Space();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("not config time frame", MessageType.Warning);
            }
        }

        protected override void KeyFrame(Vector2 pos)
        {
            float t = SeqenceWindow.inst.PiexlToTime(pos.x);
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
