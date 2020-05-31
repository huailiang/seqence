using UnityEngine;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow : EditorWindow
    {
        public static TimelineWindow inst;
        
        public Rect winArea { get; set; }
        
        public float sequencerHeaderWidth
        {
            get { return winArea.width - WindowConstants.sliderWidth; }
        }

        private void OnEnable()
        {
            state = new TimelineState(this);
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
        }

        [MenuItem("Window/Timeline", false, 1)]
        public static void ShowWindow()
        {
            inst = GetWindow<TimelineWindow>(typeof(SceneView));
            var icon = EditorGUIUtility.IconContent("TimelineAsset Icon").image as Texture2D;
            inst.titleContent = new GUIContent("  Timeline Editor", icon);
        }
    }
}
