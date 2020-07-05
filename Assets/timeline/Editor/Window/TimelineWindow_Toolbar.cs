using System;
using UnityEngine;
using PlayMode = UnityEngine.Timeline.PlayMode;

namespace UnityEditor.Timeline
{
    public partial class TimelineWindow
    {
        void TransportToolbarGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal(GUILayout.Width(WindowConstants.playmodeWidth));
            {
                PlayModeGUI();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Width(WindowConstants.sliderWidth));
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
            GUILayout.BeginHorizontal(GUILayout.Width(WindowConstants.sliderWidth));
            {
                NewButtonGUI();
                OpenButtonGUI();
                SaveButtonGUI();
                GUILayout.Space(4);
                InspectGUI();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
        }

        void PlayModeGUI()
        {
            var pmde = playMode;
            playMode = (PlayMode) EditorGUILayout.EnumPopup(playMode, EditorStyles.toolbarPopup);
            if (timeline)
            {
                if (!string.IsNullOrEmpty(state.path) && playMode != pmde && playMode != timeline.playMode)
                {
                    string tip = "current is editing" + state.path.Substring(7) + ", Are you sure change playmode?";
                    if (EditorUtility.DisplayDialog("warn", tip, "ok", "cancel"))
                    {
                        state.Dispose();
                    }
                    else
                    {
                        playMode = pmde;
                    }
                    GUIUtility.ExitGUI();
                }
                else
                {
                    timeline.playMode = playMode;
                }
            }
        }

        void GotoBeginingSequenceGUI()
        {
            if (GUILayout.Button(TimelineStyles.gotoBeginingContent, EditorStyles.toolbarButton))
            {
                state.FrameStart();
                rangeX2 = rangeX2 - rangeX1;
                rangeX1 = 0.01f;
                Repaint();
            }
        }

        void GotoEndSequenceGUI()
        {
            if (GUILayout.Button(TimelineStyles.gotoEndContent, EditorStyles.toolbarButton))
            {
                state.FrameEnd();
                float end = timeline.RecalcuteDuration();
                float len = rangeX2 - rangeX1;
                rangeX2 = end;
                rangeX1 = rangeX2 - len;
                Repaint();
            }
        }


        void PlayButtonGUI()
        {
            EditorGUI.BeginChangeCheck();
            var isPlaying = GUILayout.Toggle(state.playing, TimelineStyles.playContent, EditorStyles.toolbarButton);
            if (EditorGUI.EndChangeCheck())
            {
                state.SetPlaying(isPlaying);
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
                state.timeline?.ProcessImmediately(float.Parse(newCurrentTime));
            }
        }

        void NewButtonGUI()
        {
            if (GUILayout.Button(TimelineStyles.newContent, EditorStyles.toolbarButton))
            {
                if (state.timeline != null)
                {
                    if (EditorUtility.DisplayDialog("warn", "save current?", "save", "cancel"))
                    {
                        DoSave();
                    }
                    else
                    {
                        state.Dispose();
                    }
                }
                string dir = playMode == PlayMode.Plot ? WindowConstants.plotPath : WindowConstants.skillPath;
                string path = EditorUtility.SaveFilePanel("create timeline", dir, "timeline", "bytes");
                if (!string.IsNullOrEmpty(path))
                {
                    path = path.Substring(path.IndexOf("Assets/", StringComparison.Ordinal));
                    CreateTimeline(path);
                }
                GUIUtility.ExitGUI();
            }
        }

        void CreateTimeline(string path)
        {
            if (path.Contains(WindowConstants.plotPath))
            {
                playMode = PlayMode.Plot;
            }
            if (path.Contains(WindowConstants.skillPath))
            {
                playMode = PlayMode.Skill;
            }
            state.CreateTimeline(path, playMode);
            AssetDatabase.ImportAsset(path);
            AssetDatabase.Refresh();
        }

        void OpenButtonGUI()
        {
            if (GUILayout.Button(TimelineStyles.openContent, EditorStyles.toolbarButton))
            {
                string dir = playMode == PlayMode.Plot ? WindowConstants.plotPath : WindowConstants.skillPath;
                string path = EditorUtility.OpenFilePanel("open", dir, "xml");
                if (!string.IsNullOrEmpty(path))
                {
                    if (path.Contains(WindowConstants.plotPath))
                    {
                        playMode = PlayMode.Plot;
                    }
                    if (path.Contains(WindowConstants.skillPath))
                    {
                        playMode = PlayMode.Skill;
                    }
                    path = "Assets" + path.Replace(Application.dataPath, "");
                    state.Open(path, playMode);
                }
                GUIUtility.ExitGUI();
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
                GUIUtility.ExitGUI();
            }
        }

        void InspectGUI()
        {
            if (GUILayout.Button(TimelineStyles.refreshContent, EditorStyles.toolbarButton, GUILayout.MaxWidth(24)))
            {
                Repaint();
            }
            if (GUILayout.Button(TimelineStyles.inspectBtn, EditorStyles.toolbarButton))
            {
                TimelineInspector.ShowWindow();
            }
        }

        private void DoSave()
        {
            state.Save();
            AssetDatabase.Refresh();
        }
    }
}
