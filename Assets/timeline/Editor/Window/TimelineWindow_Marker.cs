using System;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    struct MarkAction
    {
        public Type type;
        public float posX;

        public MarkAction(Type t, float x)
        {
            type = t;
            posX = x;
        }
    }

    public partial class TimelineWindow
    {
        private Rect markderRect;
        private XMarker draging;
        internal const int markWidth = 20;
        private Event e;

        void InitializeMarkerHeader()
        {
            draging = null;
            markderRect.width = winArea.width;
            markderRect.height = WindowConstants.markerRowHeight;
        }

        void DrawMarkerDrawer()
        {
            if (state.showMarkerHeader)
            {
                DrawMarkerDrawerHeaderBackground();
                DrawMarkerDrawerHeader();
            }
            if (e == null) e = Event.current;
            if (markderRect.Contains(e.mousePosition))
            {
                switch (e.type)
                {
                    case (EventType.ContextClick):
                        if (e.button == 1)
                        {
                            GenericMenu gm = new GenericMenu();
                            var marks = TypeUtilities.GetBelongMarks(TrackType.Marker);
                            foreach (var mark in marks)
                            {
                                string str = mark.ToString();
                                int idx = str.LastIndexOf('.');
                                str = str.Substring(idx + 1);
                                var ct = EditorGUIUtility.TrTextContent("Add " + str);
                                gm.AddItem(ct, false, AddRectMark, new MarkAction(mark, e.mousePosition.x));
                            }
                            gm.ShowAsContext();
                            e.Use();
                        }
                        break;
                    case EventType.MouseDown:
                        OnMouseDown(e);
                        break;
                    case EventType.MouseUp:
                        draging = null;
                        break;
                    case EventType.MouseDrag:
                    case EventType.ScrollWheel:
                        OnMarkDrag(e);
                        break;
                }
            }
        }

        private void OnMouseDown(Event e)
        {
            float x = e.mousePosition.x;
            var tre = state.timeline.trackTrees;
            var marks = tre[0].marks;
            if (marks != null)
            {
                foreach (var mark in marks)
                {
                    float x_ = TimeToPixel(mark.time);
                    if (Mathf.Abs(x - x_) < markWidth)
                    {
                        draging = mark;
                        e.Use();
                        break;
                    }
                }
            }
        }

        private void OnMarkDrag(Event e)
        {
            float x = e.mousePosition.x;
            if (draging != null)
            {
                float x_ = TimeToPixel(draging.time);
                x_ += e.delta.x;
                x_ = Mathf.Max(0, x_);
                draging.time = TimelineWindow.inst.PiexlToTime(x_);
                e.Use();
            }
        }


        void DrawMarkerDrawerHeaderBackground()
        {
            var backgroundColor = TimelineStyles.markerHeaderDrawerBackgroundColor;
            markderRect.x = WindowConstants.rightAreaMargn;
            markderRect.y = WindowConstants.markerRowYPosition;
            markderRect.width = winArea.width;
            EditorGUI.DrawRect(markderRect, backgroundColor);
        }

        void DrawMarkerDrawerHeader()
        {
            var tre = state.timeline.trackTrees;
            if (tre != null && tre.Length > 0)
            {
                var marks = tre[0].marks;
                if (marks != null)
                {
                    foreach (var mark in marks)
                    {
                        DrawMarkItem(mark);
                    }
                }
            }
        }

        void AddRectMark(object arg)
        {
            MarkAction markAction = (MarkAction) arg;
            float time = PiexlToTime(markAction.posX);
            EditorFactory.MakeMarker(markAction.type, time);
        }

        void DrawMarkItem(XMarker mark)
        {
            float x = TimeToPixel(mark.time);
            Rect rect = markderRect;
            rect.x = x;
            rect.width = markWidth;
            GUIContent cont = state.config.GetIcon(mark.type);
            GUI.Box(rect, cont, GUIStyle.none);
        }
    }
}
