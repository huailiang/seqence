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
            showMarkerHeader = true;
            if (config == null)
            {
                config = AssetDatabase.LoadAssetAtPath<AssetConfig>(AssetConfigEditor.path);
            }
        }

        public void CreateTimeline(string path)
        {
            this.path = path;
            CleanEnv();
            RebuildInspector();
            TimelineConfig xconf = new TimelineConfig();
            xconf.tracks = new TrackData[1];
            TrackData data = new TrackData();
            data.type = AssetType.Marker;
            xconf.tracks[0] = data;
            xconf.Write(path);
            timeline = new XTimeline(xconf);
            timeline.Time = 2.0f;
            timeline.mode = TimelinePlayMode.EditorRun;
        }

        public void Open(string path)
        {
            this.path = path;
            CleanEnv();
            RebuildInspector();
            timeline = new XTimeline(path);
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
            }
        }

        public void PrevFrame()
        {
            if (timeline)
            {
                float time = timeline.Time - 1.0f / frameRate;
                timeline.ProcessImmediately(time);
            }
        }

        public void FrameStart()
        {
            timeline?.ProcessImmediately(0);
        }

        public void FrameEnd()
        {
            if (timeline)
            {
                float end = timeline.RecalcuteDuration();
                timeline.ProcessImmediately(end);
            }
        }

        private float _last = 0;
        private float _time = 0;
        private float _duration = 0;

        public void Update()
        {
            if (playing)
            {
                float t = Time.realtimeSinceStartup;
                float delta = 1.0f / frameRate;
                if (t - _last > delta)
                {
                    _time += delta;
                    if (_time >= _duration)
                    {
                        if (mode == WrapMode.Hold)
                            SetPlaying(false);
                        else
                            _time = 0;
                    }
                    SimRun();
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
                _last = Environment.TickCount;
            }
        }

        private void SimRun()
        {
            if (timeline)
            {
                timeline.Process(_time);
                window.Repaint();
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
