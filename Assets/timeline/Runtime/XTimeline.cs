using UnityEngine.Playables;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public enum TimelinePlayMode
    {
        EditorPause,
        EditorRun,
        RealRunning,
    }

    public class XTimeline
    {
        public TimelineConfig config;
        public XTrack[] trackTrees;
        public TimelinePlayMode mode;

        private float prev;
        [Range(0, 1)] public float slow = 1;
        private float delay;

        private static uint id = 0;

        public PlayableGraph graph { get; set; }

        public static uint IncID
        {
            get
            {
                if (++id == 0) id++; //overide
                return id;
            }
        }

        public float Time
        {
            get { return prev; }
            set
            {
                if (Mathf.Abs(value - prev) > 1e-4)
                {
                    ProcessImmediately(value);
                }
            }
        }

        public XMarkerTrack markerTrack
        {
            get { return trackTrees[0] as XMarkerTrack; }
        }

        public bool isRunning
        {
            get { return mode != TimelinePlayMode.EditorPause; }
        }

        public XTimeline(string path)
        {
            config = new TimelineConfig();
            config.Read(path);
            Build();
        }

        public XTimeline(TimelineConfig config)
        {
            this.config = config;
            Build();
        }

        private void Build()
        {
            delay = 1;
            graph = PlayableGraph.Create("TimelineGraph");
            var tracksData = config.tracks;
            int len = tracksData.Length;
            trackTrees = new XTrack[len];
            for (int i = 0; i < len; i++)
            {
                trackTrees[i] = XTrackFactory.Get(tracksData[i], this);
            }
            prev = 0;
            if (isRunning) graph.Play();
        }

        public void Process(float time)
        {
            if (slow < 1e-5)
            {
                //pause
            }
            else if (delay < 1)
            {
                delay += slow;
            }
            if (delay >= 1)
            {
                ProcessImmediately(time);
                delay = 0;
            }
        }


        public void ProcessImmediately(float time)
        {
            for (int i = 0; i < trackTrees.Length; i++)
            {
                trackTrees[i].Process(time, prev);
            }
            prev = time;
        }

        public void Dispose()
        {
            for (int i = 0; i < trackTrees.Length; i++)
            {
                trackTrees[i].Dispose();
            }
        }

        public float RecalcuteDuration()
        {
            float dur = 0;
            if (trackTrees != null)
            {
                for (int i = 0; i < trackTrees.Length; i++)
                {
                    var track = trackTrees[i];
                    track.ForeachHierachyTrack((trac) => trac.ForeachClip((clip) =>
                    {
                        if (clip.end > dur)
                        {
                            dur = clip.end;
                        }
                    }));
                    track.ForeachMark(mark =>
                    {
                        if (mark.time > dur)
                        {
                            dur = mark.time;
                        }
                    });
                }
            }
            return dur;
        }

        public static implicit operator bool(XTimeline timeline)
        {
            return timeline != null;
        }
    }
}
