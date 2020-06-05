using UnityEngine;

namespace UnityEditor.Timeline
{

    interface ITimelineGUI
    {
        void OnGUI();
    }

    class TimelineInspector : EditorWindow
    {
        public static void ShowWindow(TimelineWindow windw)
        {
            inst = GetWindow<TimelineInspector>("timeline inspector");
            window = windw;
        }

        static TimelineInspector inst;
        static TimelineWindow window;
        private TimelineState state
        {
            get { return window.state; }
        }

        private ITimelineGUI focus;

        public void SetActive(ITimelineGUI act)
        {
            focus = act;
        }


        private void OnEnable()
        {

        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField(state.Name);
            EditorGUILayout.IntField("fps: ", state.frameRate);
            EditorGUILayout.Toggle("playing:", state.playing);
            EditorGUILayout.EnumPopup("wrapmode:", state.mode);
            focus?.OnGUI();
            GUILayout.EndVertical();
        }


    }

}