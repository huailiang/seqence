using System;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

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

        public bool playing { get; set; }
        public bool showMarkerHeader { get; set; }

        public string Name
        {
            get { return string.IsNullOrEmpty(name) ? "tmp" : name; }
            set { name = value; }
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

        public void CreateTimeline()
        {
            TimelineConfig xconf = new TimelineConfig();
            xconf.tracks = new TrackData[1];
            TrackData data = new TrackData(TrackType.Marker);
            xconf.tracks[0] = data;
            timeline = new XTimeline(xconf);
            timeline.Time = 2.0f;
            timeline.mode = TimelinePlayMode.EditorRun;
        }

        public void Open(string path)
        {
            timeline = new XTimeline(path);
            Name = path.Replace(".bytes", "");
            int index = Name.IndexOf("Assets", StringComparison.Ordinal);
            if (index >= 0)
            {
                Name = Name.Substring(index + 7);
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

        private int _last = 0;
        private float _time = 0;
        private float _duration = 0;

        public void Update()
        {
            if (playing)
            {
                var t = Environment.TickCount;
                float delta = 1000.0f / frameRate;
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
            }
        }
    }
}
