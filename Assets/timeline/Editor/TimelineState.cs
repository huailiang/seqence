using System;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;
using Object = UnityEngine.Object;

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
        public int frameRate = 30;
        public WrapMode mode;
        private string name;
        public TimelineWindow window;
        public AssetConfig config;
        private string path;

        public bool playing { get; set; }
        public bool showMarkerHeader { get; set; }

        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(path))
                {
                    var p = path.Replace(".bytes", String.Empty);
                    return p.Substring(p.IndexOf("Assets/", StringComparison.Ordinal) + 7);
                }
                return "timeline";
            }
        }

        public void Initial()
        {
            mode = WrapMode.Hold;
            playing = false;
            _simSucc = true;
            showMarkerHeader = true;
            if (config == null)
            {
                config = AssetDatabase.LoadAssetAtPath<AssetConfig>(AssetConfigEditor.path);
            }
        }

        public void CreateTimeline(string path)
        {
            Dispose();
            this.path = path;
            TimelineConfig xconf = new TimelineConfig();
            xconf.tracks = new TrackData[1];
            TrackData data = new TrackData();
            data.type = AssetType.Marker;
            xconf.tracks[0] = data;
            xconf.Write(path);
            timeline = new XTimeline(xconf);
            AddRuntime();
            timeline.Time = 2.0f;
            timeline.mode = TimelinePlayMode.EditorRun;
            float dur = timeline.RecalcuteDuration();
            window.SetTimeRange(0, dur + 1);
        }

        public void Open(string path)
        {
            Dispose();
            _simSucc = true;
            this.path = path;
            timeline = new XTimeline(path);
            AddRuntime();
            float dur = timeline.RecalcuteDuration();
            window.SetTimeRange(0, dur + 1);
        }

        private void Dispose()
        {
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
            RuntimeTimeline runtime;
            if (go)
            {
                runtime = go.GetComponent<RuntimeTimeline>();
                if (runtime == null)
                    runtime = go.AddComponent<RuntimeTimeline>();
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
                timeline.ProcessImmediately(time);
                _simSucc = true;
            }
        }

        public void PrevFrame()
        {
            if (timeline)
            {
                float time = timeline.Time - 1.0f / frameRate;
                _simSucc = true;
                timeline.ProcessImmediately(time);
            }
        }

        public void FrameStart()
        {
            playing = false;
            _simSucc = true;
            timeline?.ProcessImmediately(0);
        }

        public void FrameEnd()
        {
            if (timeline)
            {
                playing = false;
                _simSucc = true;
                float end = timeline.RecalcuteDuration();
                timeline.ProcessImmediately(end);
            }
        }

        private float _last = 0;
        private float _time = 0;
        private float _duration = 0;
        private bool _simSucc = false;

        public void Update()
        {
            if (playing)
            {
                float t = Time.realtimeSinceStartup;
                float delta = 1.0f / frameRate;
                if (t - _last > delta)
                {
                    if (_simSucc)
                    {
                        _time += delta;
                        if (_time >= _duration)
                        {
                            if (mode == WrapMode.Hold)
                                SetPlaying(false);
                            else
                                _time = 0;
                        }
                        _simSucc = SimRun();
                    }
                    _last = t;
                }
            }
        }

        public void SetPlaying(bool play)
        {
            playing = play;
            if (play && timeline)
            {
                _duration = timeline.RecalcuteDuration();
                _time = timeline.Time;
                _last = Time.realtimeSinceStartup;
                timeline.slow = 1.0f;
                if (_time > _duration)
                {
                    _time = 0;
                }
            }
        }

        private bool SimRun()
        {
            if (timeline)
            {
                bool rt = timeline.Process(_time);
                window.Repaint();
                return rt;
            }
            return true;
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
