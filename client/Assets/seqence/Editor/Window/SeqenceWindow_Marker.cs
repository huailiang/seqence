using System;
using UnityEngine;
using UnityEngine.Seqence;
using UnityEngine.Seqence.Data;

namespace UnityEditor.Seqence
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

    public partial class SeqenceWindow
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
            DrawMarkerDrawerHeaderBackground();
            DrawMarkerDrawerHeader();
            ProcessEvent();
        }

        private void ProcessEvent()
        {
            if (e == null) e = Event.current;
            bool inMarkRect = markderRect.Contains(e.mousePosition);
            switch (e.type)
            {
                case (EventType.ContextClick):
                    if (e.button == 1 && inMarkRect)
                    {
                        GenMenu(e);
                    }
                    break;
                case EventType.MouseDown:
                    if (inMarkRect)
                    {
                        OnMouseDown(e);
                    }
                    break;
                case EventType.MouseUp:
                    if (draging != null)
                    {
                        var tre = state.seqence.trackTrees;
                        tre[0].SortMark();
                    }
                    draging = null;
                    break;
                case EventType.MouseDrag:
                case EventType.ScrollWheel:
                    OnMarkDrag(e);
                    break;
            }
        }


        private void GenMenu(Event e)
        {
            GenericMenu gm = new GenericMenu();
            var marks = TypeUtilities.GetBelongMarks(AssetType.Marker);
            foreach (var mark in marks)
            {
                string str = mark.ToString();
                int idx = str.LastIndexOf('.');
                str = str.Substring(idx + 1);
                if (str[0] == 'X') str = str.Substring(1);
                var ct = EditorGUIUtility.TrTextContent("Add " + str);
                gm.AddItem(ct, false, AddRectMark, new MarkAction(mark, e.mousePosition.x));
            }
            gm.AddSeparator("");
            var m = TrigerMark(e);
            var content = EditorGUIUtility.TrTextContent("Delete #d");
            if (m != null)
            {
                gm.AddItem(content, false, DeleteRectMark, m);
            }
            else
            {
                gm.AddDisabledItem(content, false);
            }
            gm.ShowAsContext();
            e.Use();
        }

        private void OnMouseDown(Event e)
        {
            draging = TrigerMark(e);
        }


        private XMarker TrigerMark(Event e)
        {
            float x = e.mousePosition.x;
            var marks = state.seqence.markerTrack.marks;
            if (marks != null)
            {
                foreach (var mark in marks)
                {
                    float x_ = TimeToPixel(mark.time);
                    if (Mathf.Abs(x - x_) < markWidth)
                    {
                        return mark;
                    }
                }
            }
            return null;
        }

        private void OnMarkDrag(Event e)
        {
            float x = e.mousePosition.x;
            if (draging != null)
            {
                float x_ = TimeToPixel(draging.time);
                x_ += e.delta.x;
                x_ = Mathf.Max(0, x_);
                draging.time = SeqenceWindow.inst.PiexlToTime(x_);
                e.Use();
            }
        }


        void DrawMarkerDrawerHeaderBackground()
        {
            var backgroundColor = SeqenceStyle.MarkBackground;
            markderRect.x = WindowConstants.rightAreaMargn;
            markderRect.y = WindowConstants.markerRowYPosition;
            markderRect.width = winArea.width;
            EditorGUI.DrawRect(markderRect, backgroundColor);
        }

        void DrawMarkerDrawerHeader()
        {
            var tre = state.seqence.markerTrack;
            if (tre?.marks != null)
            {
                foreach (var mark in tre.marks)
                {
                    DrawMarkItem(mark);
                }
            }
        }

        void AddRectMark(object arg)
        {
            MarkAction markAction = (MarkAction) arg;
            float time = PiexlToTime(markAction.posX);
            EditorFactory.MakeMarker(markAction.type, time);
        }

        void DeleteRectMark(object arg)
        {
            XMarker select = (XMarker) arg;
            var track = state.seqence.markerTrack;
            track.RmMarker(select);
        }

        void DrawMarkItem(XMarker mark)
        {
            if (IsTimeRange(mark.time))
            {
                float x = TimeToPixel(mark.time);
                Rect rect = markderRect;
                rect.x = x - 8;
                rect.width = markWidth;
                GUIContent cont = state.config.GetIcon(mark.type);
                GUI.Box(rect, cont, GUIStyle.none);
            }
        }
    }
}
