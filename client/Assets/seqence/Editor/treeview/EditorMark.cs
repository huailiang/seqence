using UnityEngine;
using UnityEngine.Seqence;

namespace UnityEditor.Seqence
{
    public class EditorMark : EditorObject
    {
        protected XMarker baseMarker;
        private Event e;
        private bool draging;
        private Rect rect;

        public override void OnInit(XSeqenceObject marker)
        {
            this.baseMarker = (XMarker) marker;
            draging = false;
            e = Event.current;
            OnInit();
        }

        protected virtual void OnInit()
        {
        }

        public void Inspector()
        {
            GUILayout.BeginHorizontal();
            GUIContent cont = SeqenceWindow.inst.state.config.GetIcon(baseMarker.type);
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("- " + baseMarker.type, SeqenceStyle.titleStyle);
            GUILayout.Space(4);
            GUILayout.Box(cont, GUIStyle.none, GUILayout.MaxWidth(10), GUILayout.MaxHeight(12));
            GUILayout.EndHorizontal();
            baseMarker.time = EditorGUILayout.FloatField(" time", baseMarker.time);
            baseMarker.reverse = EditorGUILayout.Toggle(" reverse", baseMarker.reverse);
            OnInspector();
            EditorGUI.indentLevel--;
        }

        protected virtual void OnInspector()
        {
        }

        public void OnGUI(Rect r)
        {
            float x = SeqenceWindow.inst.TimeToPixel(baseMarker.time);
            if (SeqenceWindow.inst.IsPiexlRange(x))
            {
                rect = r;
                rect.x = x - 8;
                rect.y = r.y + r.height / 4;
                rect.width = 20;
                GUIContent cont = SeqenceWindow.inst.state.config.GetIcon(baseMarker.type);
                GUI.Box(rect, cont, GUIStyle.none);
                ProcessEvent();
            }
        }

        public void ProcessEvent()
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (rect.Contains(e.mousePosition))
                    {
                        draging = true;
                    }
                    break;
                case EventType.MouseUp:
                    draging = false;
                    break;
                case EventType.MouseDrag:
                case EventType.ScrollWheel:
                    OnMarkDrag(e, rect);
                    break;
            }
        }


        private void OnMarkDrag(Event e, Rect rect)
        {
            if (draging)
            {
                rect.x += e.delta.x;
                baseMarker.time = SeqenceWindow.inst.PiexlToTime(rect.x);
                e.Use();
            }
        }
    }


    [SeqenceEditor(typeof(XJumpMarker))]
    public class EditorJumpMark : EditorMark
    {
        private XJumpMarker marker;

        protected override void OnInit()
        {
            this.marker = (XJumpMarker) baseMarker;
        }

        protected override void OnInspector()
        {
            marker.jump = EditorGUILayout.FloatField(" jump:", marker.jump);
        }
    }

    [SeqenceEditor(typeof(XActiveMark))]
    public class EditorActiveMark : EditorMark
    {
        private XActiveMark marker;

        protected override void OnInit()
        {
            this.marker = (XActiveMark) baseMarker;
        }

        protected override void OnInspector()
        {
            marker.active = EditorGUILayout.Toggle(" active", marker.active);
        }
    }

    [SeqenceEditor(typeof(XSlowMarker))]
    public class EditorSlowMark : EditorMark
    {
        private XSlowMarker marker;

        protected override void OnInit()
        {
            this.marker = (XSlowMarker) baseMarker;
        }

        protected override void OnInspector()
        {
            marker.slow = EditorGUILayout.FloatField(" slowRate", marker.slow);
        }
    }
}
