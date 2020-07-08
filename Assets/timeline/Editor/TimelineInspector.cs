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
        private Vector2 scroll;

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
                    EditorGUILayout.LabelField("frame rate: \t" + state.frameRate);
                    EditorGUILayout.LabelField("play status:\t" + state.playing);
                    state.mode = (WrapMode) EditorGUILayout.EnumPopup("wrapmode:", state.mode);
                    scroll = EditorGUILayout.BeginVerticalScrollView(scroll);
                    GUIMark();
                    GUITracks();
                    EditorGUILayout.EndScrollView();
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
                    emarks[i] = (EditorMark) TypeUtilities.InitEObject(marks[i]);
                }
            }
            if (emarks != null)
            {
                using (GUIColorOverride color = new GUIColorOverride(Color.green))
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

        public void OnRebuild()
        {
            emarks = null;
            markF = false;
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
