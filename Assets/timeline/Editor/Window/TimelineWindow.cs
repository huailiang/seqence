using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Timeline
{
    [EditorWindowTitle(title = "Timeline", useTypeNameAsIconName = true)]
    public partial class TimelineWindow : EditorWindow
    {
        public static TimelineWindow inst;
        private EditorTrackTree tree;
        public Rect winArea { get; set; }

        public Rect centerArea { get; set; }

        readonly List<Manipulator> m_CaptureSession = new List<Manipulator>();


        public float sequencerHeaderWidth
        {
            get { return winArea.width - WindowConstants.sliderWidth; }
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
                TimelineHeaderGUI();
                DrawMarkerDrawer();
                tree.OnGUI(state);
                TimelineTimeAreaGUI();
            }
            else
            {
                CalculCenter();
                EditorGUI.LabelField(centerArea, TimelineStyles.createNewTimelineText);
            }
            winArea = position;
        }

        public void AddCaptured(Manipulator manipulator)
        {
            if (!m_CaptureSession.Contains(manipulator)) m_CaptureSession.Add(manipulator);
        }

        public void RemoveCaptured(Manipulator manipulator)
        {
            m_CaptureSession.Remove(manipulator);
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
