using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    [TimelineEditor(typeof(XGroupTrack))]
    public class EditorGroupTrack : EditorTrack
    {
        protected override bool ignoreDraw
        {
            get { return true; }
        }

        protected override Color trackColor
        {
            get { return new Color(0.3f, 0.7f, 0.8f, 0.3f); }
        }

        protected override string trackHeader
        {
            get { return "Group" + ID; }
        }


        protected override void OnGUIContent()
        {
            base.OnGUIContent();
            Rect rt = rect;
            rt.x = head.x;
            rt.width = TimelineWindow.inst.winArea.width;
            EditorGUI.DrawRect(rt, trackColor);

            GroupMenu();
        }


        private void GroupMenu()
        {
            var e = Event.current;
            if (e.type == EventType.ContextClick)
            {
                if (triger)
                {
                    GenericMenu pm = new GenericMenu();
                    var types = TypeUtilities.AllRootTrackExcMarkers();
                    for (int i = 0; i < types.Count; i++)
                    {
                        string str = types[i].ToString();
                        int idx = str.LastIndexOf('.');
                        if (idx >= 0)
                        {
                            str = "Add " + str.Substring(idx + 1);
                        }
                        pm.AddItem(EditorGUIUtility.TrTextContent(str), false, OnAddTrackItem, types[i]);
                    }
                    if (track.childs?.Length > 0)
                    {
                        pm.AddSeparator("");
                        pm.AddItem(EditorGUIUtility.TrTextContent("Delete Track"), false, DeleteTrack);
                    }
                    pm.ShowAsContext();
                }
            }
        }


        private void OnAddTrackItem(object arg)
        {
            Type type = (Type) arg;
            TrackData data = EditorFactory.CreateTrackData(type);
            var state = TimelineWindow.inst.state;
            var tr = XTimelineFactory.GetTrack(data, state.timeline, this.track);
            var tmp = track;
            if (track.childs?.Length > 0)
            {
                tmp = track.childs.Last();
            }
            tr.parent.AddSub(tr);
            tr.parent.AddTrackChildData(data);
            int idx = TimelineWindow.inst.tree.IndexOfTrack(tmp);
            TimelineWindow.inst.tree.AddTrack(tr, idx + 1);
        }
    }
}
