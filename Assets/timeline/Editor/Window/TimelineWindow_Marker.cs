using System;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    public partial class TimelineWindow
    {
        private Rect markderRect;

        void InitializeMarkerHeader()
        {
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
            var e = Event.current;
            if (markderRect.Contains(e.mousePosition))
            {
                if (e.type == EventType.MouseUp)
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
                }
            }
        }

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

        void DrawMarkerDrawerHeaderBackground()
        {
            var backgroundColor = TimelineStyles.markerHeaderDrawerBackgroundColor;
            markderRect.x = winArea.x + WindowConstants.rightAreaMargn;
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
            rect.width = 20;
            GUIContent cont = state.config.GetIcon(mark.type);
            GUI.Box(rect, cont, GUIStyle.none);
        }
    }
}
