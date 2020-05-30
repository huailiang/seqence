using UnityEngine;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow
    {
        public TimelineState state { get; private set; }


        public void InitialToolbar()
        {
            state = TimelineState.inst;
        }

        void TransportToolbarGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal(GUILayout.Width(80));
            {
                GotoBeginingSequenceGUI();
                PreviousEventButtonGUI();
                PlayButtonGUI();
                NextEventButtonGUI();
                GotoEndSequenceGUI();
                GUILayout.FlexibleSpace();
                TimeCodeGUI();
            }
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(state.Name);
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal(GUILayout.Width(80));
            {
                NewButtonGUI();
                OpenButtonGUI();
                SaveButtonGUI();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
        }

        void GotoBeginingSequenceGUI()
        {
            if (GUILayout.Button(TimelineStyles.gotoBeginingContent, EditorStyles.toolbarButton))
            {
                state.FrameStart();
            }
        }

        void GotoEndSequenceGUI()
        {
            if (GUILayout.Button(TimelineStyles.gotoEndContent, EditorStyles.toolbarButton))
            {
                state.FrameEnd();
            }
        }


        void PlayButtonGUI()
        {
            EditorGUI.BeginChangeCheck();
            var isPlaying = GUILayout.Toggle(state.playing, TimelineStyles.playContent, EditorStyles.toolbarButton);
            if (EditorGUI.EndChangeCheck())
            {
                state.playing = isPlaying;
            }
        }

        void NextEventButtonGUI()
        {
            if (GUILayout.Button(TimelineStyles.nextFrameContent, EditorStyles.toolbarButton))
            {
                state.NextFrame();
            }
        }

        void PreviousEventButtonGUI()
        {
            if (GUILayout.Button(TimelineStyles.previousFrameContent, EditorStyles.toolbarButton))
            {
                state.PrevFrame();
            }
        }

        void TimeCodeGUI()
        {
            EditorGUI.BeginChangeCheck();

            string currentTime = state.timeline != null ? state.timeline.Time.ToString("F1") : "0";
            var r = EditorGUILayout.GetControlRect(false, EditorGUI.kSingleLineHeight, EditorStyles.toolbarTextField,
                GUILayout.MinWidth(WindowConstants.minTimeCodeWidth));
            var id = GUIUtility.GetControlID("RenameFieldTextField".GetHashCode(), FocusType.Passive, r);
            var newCurrentTime = EditorGUI.DelayedTextFieldInternal(r, id, GUIContent.none, currentTime, null,
                EditorStyles.toolbarTextField);

            if (EditorGUI.EndChangeCheck())
            {
                state.timeline?.Process(float.Parse(newCurrentTime));
            }
        }

        void NewButtonGUI()
        {
            if (GUILayout.Button(TimelineStyles.newContent, EditorStyles.toolbarButton))
            {
                if (state.timeline != null)
                {
                    if (EditorUtility.DisplayDialog("warn", "save current", "save", "cancel"))
                    {
                        DoSave();
                    }
                    else
                    {
                        state.CreateTimeline();
                    }
                }
                else
                {
                    state.CreateTimeline();
                }
            }
        }

        void OpenButtonGUI()
        {
            if (GUILayout.Button(TimelineStyles.openContent, EditorStyles.toolbarButton))
            {
                string path = EditorUtility.OpenFilePanel("open", "Assets/", "bytes");
                state.Open(path);
            }
        }

        void SaveButtonGUI()
        {
            if (GUILayout.Button(TimelineStyles.saveContent, EditorStyles.toolbarButton))
            {
                if (state.timeline == null)
                {
                    EditorUtility.DisplayDialog("warn", "not create timeline in editor", "ok");
                }
                else
                {
                    DoSave();
                }
            }
        }

        private void DoSave()
        {
            string path = EditorUtility.SaveFilePanel("save", "Assets/", "timeline", "bytes");
            state.timeline.config.Write(path);
            AssetDatabase.ImportAsset(path);
            AssetDatabase.Refresh();
        }
    }
}
