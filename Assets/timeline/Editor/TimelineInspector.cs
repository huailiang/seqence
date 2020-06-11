using System.Collections.Generic;
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
            inst = GetWindow<TimelineInspector>("timeline inspector");
        }

        public static TimelineInspector inst;

        private TimelineState state
        {
            get { return TimelineWindow.inst?.state ?? null; }
        }

        private List<ITimelineInspector> focus;

        private void OnEnable()
        {
            focus = new List<ITimelineInspector>();
        }

        private void OnDisable()
        {
            focus.Clear();
        }

        private void OnDestroy()
        {
            focus.Clear();
        }

        public void SetActive(ITimelineInspector act, bool show = true)
        {
            if (!focus.Contains(act))
            {
                if (show)
                {
                    focus.Add(act);
                }
            }
            else
            {
                if (!show)
                {
                    focus.Remove(act);
                }
            }
        }

        public void Remove(ITimelineInspector act)
        {
            if (focus.Contains(act))
            {
                focus.Remove(act);
            }
        }


        private void OnGUI()
        {
            inst = this;
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField(state.Name);
            EditorGUILayout.IntField("fps: ", state.frameRate);
            EditorGUILayout.Toggle("playing:", state.playing);
            EditorGUILayout.EnumPopup("wrapmode:", state.mode);
            foreach (var item in focus)
            {
                item.OnInspector();
            }
            GUILayout.EndVertical();
        }
    }
}
