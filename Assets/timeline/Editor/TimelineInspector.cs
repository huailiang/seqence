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

        private void GUIMark()
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
                if (markF)
                {
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
