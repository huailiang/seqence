using UnityEngine;

namespace UnityEditor.Seqence
{
    interface ITimelineInspector
    {
        void OnInspector();
    }

    class SeqenceInspector : EditorWindow
    {
        public static SeqenceInspector inst;
        private Vector2 scroll;

        public static void ShowWindow()
        {
            inst = GetWindow<SeqenceInspector>("seqence inspector");
            inst.Show();
        }

        private void OnGUI()
        {
            inst = this;
            if (SeqenceWindow.inst)
            {
                var state = SeqenceWindow.inst.state;
                if (state != null && state.seqence)
                {
                    GUILayout.BeginVertical();
                    EditorGUILayout.LabelField(state.Name);
                    EditorGUILayout.LabelField("frame rate:", state.frameRate.ToString());
                    EditorGUILayout.LabelField("play status:", state.playing.ToString());
                    state.mode = (WrapMode) EditorGUILayout.EnumPopup("wrapmode:", state.mode);
                    scroll = EditorGUILayout.BeginVerticalScrollView(scroll);
                    GUIMark();
                    GUITracks();
                    EditorGUILayout.EndScrollView();
                    GUILayout.EndVertical();
                }
            }
            else
            {
                EditorGUILayout.LabelField("no timeline select");
            }
        }


        private bool markF;
        private EditorMark[] emarks;

        private void GUIMark()
        {
            var timeline = SeqenceWindow.inst.timeline;
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
                using (new GUIColorOverride(Color.green))
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
            var trees = SeqenceWindow.inst.tree;
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
