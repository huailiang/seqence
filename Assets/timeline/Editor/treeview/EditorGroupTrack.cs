using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    [TimelineEditor(typeof(XGroupTrack))]
    public class EditorGroupTrack : EditorTrack
    {
        private Rect area;

        private string _title = String.Empty;

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
            get
            {
                if (string.IsNullOrEmpty(_title))
                {
                    return "Group" + ID;
                }
                else
                {
                    return _title + " " + ID;
                }
            }
        }


        protected override void OnGUIContent()
        {
            base.OnGUIContent();
            area = RenderRect;
            area.x = RenderHead.x;
            area.width = TimelineWindow.inst.winArea.width;
            EditorGUI.DrawRect(area, trackColor);

            GroupMenu();
        }


        private void GroupMenu()
        {
            var e = Event.current;
            if (e.type == EventType.ContextClick)
            {
                if (area.Contains(e.mousePosition))
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
                    var del = EditorGUIUtility.TrTextContent("Delete Track");
                    var dels = EditorGUIUtility.TrTextContent("Delete Tracks (Childs Include)");
                    pm.AddSeparator("");
                    if (track.childs == null || track.childs.Length <= 0)
                    {
                        pm.AddItem(del, false, DeleteTrack);
                        pm.AddDisabledItem(dels, false);
                    }
                    else
                    {
                        pm.AddDisabledItem(del, false);
                        pm.AddItem(dels, false, DeleteTrack);
                    }
                    pm.ShowAsContext();
                    e.Use();
                }
            }
        }


        protected override void OnInspectorTrack()
        {
            base.OnInspectorTrack();
            _title = EditorGUILayout.TextField(" comment:", _title);
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
