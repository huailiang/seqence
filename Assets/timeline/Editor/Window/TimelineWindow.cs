using UnityEngine;

namespace UnityEditor.Timeline
{
    [EditorWindowTitle(title = "Timeline", useTypeNameAsIconName = true)]
    public partial class TimelineWindow : EditorWindow
    {
        public static TimelineWindow inst;
        
        public EditorTrackTree tree;
        
        public Rect winArea { get; set; }

        public Rect centerArea { get; set; }


        private void OnEnable()
        {
            state = new TimelineState(this);
            tree = new EditorTrackTree();
            InitializeTimeArea();
            InitializeMarkerHeader();
        }

        private void Update()
        {
            if (inst == null) inst = this;
            state?.Update();
        }

        void OnGUI()
        {
            TransportToolbarGUI();
            if (state.timeline)
            {
                TimelineTimeAreaGUI();
                TimelineHeaderGUI();
                DrawMarkerDrawer();
                tree.OnGUI(state);
                DrawTimeOnSlider();
                DrawSptLine();
            }
            else
            {
                CalculCenter();
                EditorGUI.LabelField(centerArea, TimelineStyles.createNewTimelineText);
            }
            winArea = position;
        }

        private void DrawSptLine()
        {
            Color c = TimelineStyles.timeCursor.normal.textColor * 0.6f;
            float x = WindowConstants.sliderWidth + 2;
            Rect rec = new Rect(x, WindowConstants.timeAreaYPosition, 1, tree.TracksBtmY);
            EditorGUI.DrawRect(rec, c);
        }

        private void CalculCenter()
        {
            float x = position.width / 2 - 100;
            float y = position.height / 2;
            float width = position.width / 2;
            centerArea = new Rect(x, y, width, 40);
        }

        [MenuItem("Assets/Create/Timeline", false, 450)]
        public static void CreateNewTimeline()
        {
            ShowWindow();
            inst.state?.CreateTimeline();
        }

        [MenuItem("Window/Timeline", false, 1)]
        public static void ShowWindow()
        {
            inst = GetWindow<TimelineWindow>(typeof(SceneView));
            inst.titleContent = EditorGUIUtility.IconContent("TimelineAsset Icon", "Timeline");
            inst.titleContent.text = "  Timeline";
        }
    }
}
