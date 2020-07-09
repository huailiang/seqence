using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public class EditorMark : EditorObject
    {
        protected XMarker baseMarker;
        private Event e;
        private bool draging;
        private Rect rect;

        public override void OnInit(XTimelineObject marker)
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
            GUIContent cont = TimelineWindow.inst.state.config.GetIcon(baseMarker.type);
            EditorGUILayout.LabelField(baseMarker.type.ToString(), TimelineStyles.titleStyle);
            GUILayout.Space(4);
            GUILayout.Box(cont, GUIStyle.none, GUILayout.MaxWidth(10), GUILayout.MaxHeight(12));
            GUILayout.EndHorizontal();

            baseMarker.time = EditorGUILayout.FloatField(" time", baseMarker.time);
            baseMarker.reverse = EditorGUILayout.Toggle(" reverse", baseMarker.reverse);
            OnInspector();
        }

        protected virtual void OnInspector()
        {
        }

        public void OnGUI(Rect r)
        {
            float x = TimelineWindow.inst.TimeToPixel(baseMarker.time);
            if (TimelineWindow.inst.IsPiexlRange(x))
            {
                rect = r;
                rect.x = x - 8;
                rect.y = r.y + r.height / 4;
                rect.width = 20;
                GUIContent cont = TimelineWindow.inst.state.config.GetIcon(baseMarker.type);
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
                baseMarker.time = TimelineWindow.inst.PiexlToTime(rect.x);
                e.Use();
            }
        }
    }


    [TimelineEditor(typeof(XJumpMarker))]
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

    [TimelineEditor(typeof(XActiveMark))]
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

    [TimelineEditor(typeof(XSlowMarker))]
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
