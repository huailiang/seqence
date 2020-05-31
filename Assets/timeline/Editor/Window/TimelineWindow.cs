using UnityEngine;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow : EditorWindow
    {
        public Rect winArea { get; set; }

        private void OnEnable()
        {
            InitializeTimeArea();
            InitialToolbar();
            InitializeMarkerHeader();
        }

        void OnGUI()
        {
            TimelineTimeAreaGUI();
            TransportToolbarGUI();
            TimelineHeaderGUI();
            DrawMarkerDrawer();
            winArea = position;
        }


        [MenuItem("Assets/Create/Timeline", false, 450)]
        public static void CreateNewTimeline()
        {
            uint id = uint.MaxValue;
            Debug.Log(id);
            id++;
            Debug.Log(id);
        }

        [MenuItem("Window/Timeline", false, 1)]
        public static void ShowWindow()
        {
            var win = GetWindow<TimelineWindow>(typeof(SceneView));
            var icon = EditorGUIUtility.IconContent("TimelineAsset Icon").image as Texture2D;
            win.titleContent = new GUIContent("  Timeline Editor", icon);
        }
    }
}
