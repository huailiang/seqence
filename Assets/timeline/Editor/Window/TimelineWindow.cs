using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [EditorWindowTitle(title = "Timeline", useTypeNameAsIconName = true)]
    public partial class TimelineWindow : EditorWindow
    {
        public static TimelineWindow inst;

        public EditorTrackTree tree;

        public Rect winArea { get; set; }

        public Rect centerArea { get; set; }

        public XTimeline timeline
        {
            get { return inst.state.timeline; }
        }

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
                EventMenuHandler();
            }
            else
            {
                CalculWindowCenter();
                EditorGUI.LabelField(centerArea, TimelineStyles.createNewTimelineText);
            }
            winArea = position;
        }


        private Vector2 sc;

        private void EventMenuHandler()
        {
            Rect rt = winArea;
            rt.y = tree.TracksBtmY;
            e = Event.current;
            if (e.type == EventType.ContextClick && rt.Contains(e.mousePosition))
            {
                GenCustomMenu();
            }
            else if (e.type == EventType.Layout)
            {
                if (TimelineInspector.inst != null) TimelineInspector.inst.Repaint();
            }
        }

        private void DrawSptLine()
        {
            Color c = TimelineStyles.timeCursor.normal.textColor * 0.6f;
            float x = WindowConstants.sliderWidth + 2;
            Rect rec = new Rect(x, WindowConstants.timeAreaYPosition, 1,
                tree.TracksBtmY - WindowConstants.timeAreaYPosition - 2);
            EditorGUI.DrawRect(rec, c);
        }

        private void CalculWindowCenter()
        {
            float x = position.width / 2 - 100;
            float y = position.height / 2;
            float width = position.width / 2;
            centerArea = new Rect(x, y, width, 40);
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
