using System;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;
using Object = UnityEngine.Object;
using PlayMode = UnityEngine.Timeline.PlayMode;

namespace UnityEditor.Timeline
{
    public enum WrapMode
    {
        Loop,
        Hold
    }

    public class TimelineState
    {
        public TimelineState(TimelineWindow win)
        {
            window = win;
            Initial();
        }

        public XTimeline timeline;
        public WrapMode mode;
        private string name;
        public TimelineWindow window;
        public AssetConfig config;
        public string path;

        public bool showMarkerHeader { get; set; }

        public int frameRate
        {
            get { return XTimeline.frameRate; }
        }

        public bool playing
        {
            get { return timeline?.playing ?? false; }
        }

        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(path))
                {
                    var p = path.Replace(".bytes", "").Replace(".xml", "");
                    return p.Substring(p.LastIndexOf("/", StringComparison.Ordinal) + 1);
                }
                return "***";
            }
        }

        public void Initial()
        {
            mode = WrapMode.Hold;
            showMarkerHeader = true;
            if (config == null)
            {
                config = AssetDatabase.LoadAssetAtPath<AssetConfig>(AssetConfigEditor.path);
            }
            EditorApplication.playModeStateChanged -= OnPlayChanged;
            EditorApplication.playModeStateChanged += OnPlayChanged;
        }

        private void OnPlayChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode || state == PlayModeStateChange.ExitingPlayMode ||
                state == PlayModeStateChange.EnteredEditMode)
            {
                timeline?.Dispose();
                timeline = null;
                CleanEnv();
                if (window != null)
                {
                    window.Dispose();
                    window.Repaint();
                }
            }
        }

        public void CheckExist()
        {
            if (timeline == null && Application.isPlaying)
            {
                var run = Object.FindObjectOfType<RuntimeTimeline>();
                if (run) timeline = run.timeline;
            }
        }

        public void CreateTimeline(string path, PlayMode mode)
        {
            Dispose();
            this.path = path;
            TimelineConfig xconf = new TimelineConfig();
            xconf.tracks = new TrackData[1];
            TrackData data = new TrackData();
            data.type = AssetType.Marker;
            xconf.tracks[0] = data;
            xconf.Write(path);
            timeline = new XTimeline(xconf, mode);
            timeline.Finish = OnPlayFinish;
            AddRuntime();
            timeline.Time = 2.0f;
            timeline.editMode = TimelinePlayMode.EditorRun;
        }

        public void Open(string path, PlayMode mode)
        {
            Dispose();
            this.path = path;
            timeline = new XTimeline(path, mode);
            timeline.Finish = OnPlayFinish;
            AddRuntime();
            float dur = timeline.RecalcuteDuration();
            window.SetTimeRange(0, dur * 1.5f);
        }

        public void Dispose()
        {
            path = string.Empty;
            CleanEnv();
            RebuildInspector();
            timeline?.Dispose();
            if (window != null)
            {
                window.Dispose();
            }
        }

        private void AddRuntime()
        {
            var go = GameObject.Find("timeline");
            if (go)
            {
                var runtime = go.GetComponent<RuntimeTimeline>();
                if (runtime == null) runtime = go.AddComponent<RuntimeTimeline>();
                runtime.path = this.path;
                runtime.timeline = timeline;
            }
        }


        public void Save()
        {
            if (!string.IsNullOrEmpty(path))
            {
                timeline.BuildConf();
                timeline.config.Write(path);
                var p = path.Replace(".bytes", ".xml");
                timeline.config.WriteXml(p);
                RebuildInspector();
            }
            else
            {
                EditorUtility.DisplayDialog("warn", "save path is null", "ok");
            }
        }

        private void RebuildInspector()
        {
            var insp = TimelineInspector.inst;
            if (insp)
            {
                insp.OnRebuild();
            }
        }

        public void NextFrame()
        {
            if (timeline)
            {
                float time = timeline.Time + 1.0f / frameRate;
                timeline.EditorCheckPlay();
                timeline.ProcessImmediately(time);
            }
        }

        public void PrevFrame()
        {
            if (timeline)
            {
                float time = timeline.Time - 1.0f / frameRate;
                timeline.EditorCheckPlay();
                timeline.ProcessImmediately(time);
            }
        }

        public void FrameStart()
        {
            timeline.SetPlaying(false);
            timeline?.ProcessImmediately(0);
        }

        public void FrameEnd()
        {
            if (timeline)
            {
                timeline.SetPlaying(false);
                float end = timeline.RecalcuteDuration();
                timeline.ProcessImmediately(end);
            }
        }


        public void Update()
        {
            timeline?.Update();
            if (playing)
            {
                window.Repaint();
            }
        }

        private void OnPlayFinish()
        {
            SetPlaying(mode == WrapMode.Loop);
        }

        public void SetPlaying(bool play)
        {
            timeline.SetPlaying(play);
            if (play)
            {
                timeline.EditorCheckPlay();
            }
        }

        public static void CleanEnv()
        {
            var go = GameObject.Find("timeline");
            if (go)
            {
                Object.DestroyImmediate(go);
            }
        }
    }
}
