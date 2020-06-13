using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
    interface ITimelineInspector
    {
        void OnInspector();
    }

    class TimelineInspector : EditorWindow
    {
        public static TimelineInspector inst;

        public static void ShowWindow()
        {
            inst = GetWindow<TimelineInspector>("timeline inspector");
            inst.Show();
        }

        private void OnGUI()
        {
            inst = this;
            if (TimelineWindow.inst)
            {
                var state = TimelineWindow.inst.state;
                if (state != null)
                {
                    GUILayout.BeginVertical();
                    EditorGUILayout.LabelField(state.Name);
                    EditorGUILayout.IntField("fps: ", state.frameRate);
                    EditorGUILayout.Toggle("playing:", state.playing);
                    EditorGUILayout.EnumPopup("wrapmode:", state.mode);
                    GUIMark();
                    GUITracks();
                    GUILayout.EndVertical();
                }
            }
        }


        private bool markF;
        private EditorMark[] emarks;

        private void GUIMark()
        {
            var timeline = TimelineWindow.inst.timeline;
            var marks = timeline?.trackTrees?[0].marks;
            if (marks != null && (emarks == null || emarks.Length != marks.Length))
            {
                int len = marks.Length;
                emarks = new EditorMark[len];
                for (int i = 0; i < len; i++)
                {
                    var t = TypeUtilities.GetEditorAsset(marks[i].GetType());
                    emarks[i] = (EditorMark) Activator.CreateInstance(t);
                }
            }
            if (emarks != null)
            {
                using (GUIColorOverride color = new GUIColorOverride(Color.red))
                {
                    markF = EditorGUILayout.Foldout(markF, "marks");
                }
                if (markF)
                {
                    foreach (var mark in emarks)
                    {
                        mark.Inspector();
                    }
                }
            }
            GUILayout.Space(4);
        }

        private void GUITracks()
        {
            var trees = TimelineWindow.inst.tree;
            if (trees?.hierachy != null)
            {
                foreach (var track in trees.hierachy)
                {
                    ITimelineInspector gui = (ITimelineInspector) track;
                    gui.OnInspector();
                    GUILayout.Space(4);
                }
            }
        }
    }
}
