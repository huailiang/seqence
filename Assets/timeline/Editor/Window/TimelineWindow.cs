using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Timeline
{

    [EditorWindowTitle(title = "Timeline", useTypeNameAsIconName = true)]
    public partial class TimelineWindow : EditorWindow
    {
        
        public static TimelineWindow inst;
        
        public Rect winArea { get; set; }
        
        readonly List<Manipulator> m_CaptureSession = new List<Manipulator>();
        
        
        public float sequencerHeaderWidth
        {
            get { return winArea.width - WindowConstants.sliderWidth; }
        }

        private void OnEnable()
        {
            state = new TimelineState(this);
            InitializeTimeArea();
            InitializeMarkerHeader();
        }

        private void Update()
        {
            state?.Update();
        }

        void OnGUI()
        {
            TimelineTimeAreaGUI();
            TransportToolbarGUI();
            TimelineHeaderGUI();
            DrawMarkerDrawer();
            winArea = position;
        }

        public void AddCaptured(Manipulator manipulator)
        {
            if (!m_CaptureSession.Contains(manipulator))
                m_CaptureSession.Add(manipulator);
        }

        public void RemoveCaptured(Manipulator manipulator)
        {
            m_CaptureSession.Remove(manipulator);
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
            inst.titleContent = EditorGUIUtility.IconContent("TimelineAsset Icon","Timeline");
            inst.titleContent.text = "  Timeline";
        }

    }

}
