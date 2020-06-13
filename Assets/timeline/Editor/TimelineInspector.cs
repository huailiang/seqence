using UnityEngine;

namespace UnityEditor.Timeline
{
    interface ITimelineInspector
    {
        void OnInspector();
    }

    class TimelineInspector : EditorWindow
    {
        public static void ShowWindow()
        {
            GetWindow<TimelineInspector>("timeline inspector");
        }

        private void OnGUI()
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
                GUISelect();
                GUILayout.EndVertical();
            }
        }


        private bool markF;

        private void GUIMark()
        {
            if (markF)
            {
                var timeline = TimelineWindow.inst.timeline;
                var marks = timeline?.trackTrees?[0].marks;
                if (marks != null)
                {
                    int i = 0;
                    using (GUIColorOverride color = new GUIColorOverride(Color.red))
                    {
                        markF = EditorGUILayout.Foldout(markF, "marks");
                    }
                    foreach (var mark in marks)
                    {
                        EditorGUILayout.LabelField(++i + ": " + mark.type);
                        mark.time = EditorGUILayout.FloatField("time", mark.time);
                        mark.reverse = EditorGUILayout.Toggle("reverse", mark.reverse);
                    }
                }
            }
            GUILayout.Space(4);
        }

        private void GUISelect()
        {
            var trees = TimelineWindow.inst.tree;
            if (trees != null && trees.hierachy != null)
            {
                foreach (var track in trees.hierachy)
                {
                    if (track.@select)
                    {
                        ITimelineInspector gui = track as ITimelineInspector;
                        gui.OnInspector();
                        GUILayout.Space(4);
                    }
                }
            }
        }
    }
}
