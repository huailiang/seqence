using UnityEngine;
using UnityEngine.Timeline;
using PlayMode = UnityEngine.Timeline.PlayMode;

namespace UnityEditor.Timeline
{

    public partial class SeqenceWindow : EditorWindow
    {
        public static SeqenceWindow inst;

        public EditorTrackTree tree;

        public Rect winArea { get; set; }

        public Rect centerArea { get; set; }

        
        public PlayMode playMode;

        public XSeqence timeline
        {
            get { return inst.state.seqence; }
        }

        [Callbacks.DidReloadScripts]
        static void OnEditorReload()
        {
            SeqenceState.CleanEnv();
        }

        private void OnEnable()
        {
            state = new SeqenceState(this);
            tree = new EditorTrackTree();
            InitializeTimeArea();
            InitializeMarkerHeader();
        }

        private void Update()
        {
            state?.Update();
        }

        void OnGUI()
        {
            if (inst == null) inst = this;
            TransportToolbarGUI();
            state?.CheckExist();
            if (state.seqence)
            {
                TimelineTimeAreaGUI();
                TimelineHeaderGUI();
                DrawMarkerDrawer();
                tree.OnGUI(state);
                DrawTimeOnSlider();
                DrawSptLine();
                DrawEndLine();
                EventHandler();
            }
            else
            {
                CalculWindowCenter();
                EditorGUI.LabelField(centerArea, SeqenceStyle.createNewTimelineText);
            }
            winArea = position;
        }

        public void Dispose()
        {
            tree?.Dispose();
        }


        private Vector2 sc;

        private void EventHandler()
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
                if (SeqenceInspector.inst != null) SeqenceInspector.inst.Repaint();
            }
            else if (e.type == EventType.MouseUp)
            {
                timeline.RecalcuteDuration();
                Repaint();
            }
        }

        private void DrawSptLine()
        {
            Color c = SeqenceStyle.timeCursor.normal.textColor * 0.6f;
            float x = WindowConstants.sliderWidth + 2;
            Rect rec = new Rect(x, WindowConstants.timeAreaYPosition, 1,
                tree.TracksBtmY - WindowConstants.timeAreaYPosition - 2);
            EditorGUI.DrawRect(rec, c);
        }

        private void DrawEndLine()
        {
            if (timeline)
            {
                Color c = SeqenceStyle.colorEndLine;

                float x = TimeToPixel(timeline.Duration);
                if (timeline.Duration > 1e-1 && x > WindowConstants.sliderWidth)
                {
                    Rect rec = new Rect(x, WindowConstants.timeAreaYPosition, 1,
                        tree.TracksBtmY - WindowConstants.timeAreaYPosition - 2);
                    EditorGUI.DrawRect(rec, c);
                }
            }
        }


        private void CalculWindowCenter()
        {
            float x = position.width / 2 - 100;
            float y = position.height / 2;
            float width = position.width / 2;
            centerArea = new Rect(x, y, width, 40);
        }


        [MenuItem("Window/Seqence", false, 1)]
        public static void ShowWindow()
        {
            inst = GetWindow<SeqenceWindow>(typeof(SceneView));
            inst.titleContent = new GUIContent("Seqence", "Seqence Editor");
        }
    }
}
