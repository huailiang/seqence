using System;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    class TimelineState
    {
        public TimelineState(TimelineWindow win)
        {
            window = win;
            Initial();
        }

        public TimelineWindow window;

        public bool playing { get; set; }
        public bool recording { get; set; }
        public bool showMarkerHeader { get; set; }

        public XTimeline timeline;

        public int frameRate = 30;

        private string name;

        public string Name
        {
            get { return string.IsNullOrEmpty(name) ? "tmp" : name; }
            set { name = value; }
        }

        public void Initial()
        {
            playing = false;
            showMarkerHeader = true;
        }

        public void CreateTimeline()
        {
            TimelineConfig config = new TimelineConfig();
            config.tracks = new TrackData[1];
            TrackData data = new TrackData();
            data.type = TrackType.Marker;
            config.tracks[0] = data;
            timeline = new XTimeline(config);
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
                timeline.Process(time);
            }
        }

        public void PrevFrame()
        {
            if (timeline)
            {
                float time = timeline.Time - 1.0f / frameRate;
                timeline.Process(time);
            }
        }

        public void FrameStart()
        {
            timeline?.Process(0);
        }

        public void FrameEnd()
        {
            if (timeline)
            {
                float end = timeline.RecalcuteDuration();
                timeline.Process(end);
            }
        }
    }
}
