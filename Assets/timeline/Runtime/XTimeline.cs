using System;
using UnityEngine.Animations;
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

    public enum PlayMode { Plot, Skill }

    public class XTimelineObject
    {
        public static implicit operator bool(XTimelineObject obj)
        {
            return obj != null;
        }
    }

    public class XTimeline
    {
        public TimelineConfig config;
        public XTrack[] trackTrees;
        public TimelinePlayMode editMode;
        public PlayMode playMode;
        private GameObject timelineRoot;
        private float _time = 0;
        private float _duration = 0;
        public System.Action Finish;

        private float prev;
        [Range(0, 1)] public float slow = 1;
        private float delay;

        public const int frameRate = 30;
        private float _last = 0;

        public bool playing { get; set; }

        public bool blending { get; set; }

        public XTrack SkillHostTrack;

        private static uint id = 0;

        public static PlayableGraph graph { get; set; }

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
            get
            {
                if (trackTrees != null && trackTrees.Length > 0)
                    return trackTrees[0] as XMarkerTrack;
                else
                    return null;
            }
        }

        public bool isRunningMode
        {
            get { return editMode != TimelinePlayMode.EditorPause; }
        }

        public float Duration
        {
            get { return _duration; }
        }

        public XTimeline(TimelineConfig conf, PlayMode mode)
        {
            Initial(conf, mode, false);
        }

        public XTimeline(string path, PlayMode mode)
        {
            ReadConf(path);
            if (config != null)
            {
                Initial(config, mode, false);
            }
        }

        private void ReadConf(string path)
        {
            if (path.EndsWith(".xml"))
            {
                config = TimelineConfig.ReadXml(path);
            }
            else if (string.IsNullOrEmpty(path))
            {
                config = new TimelineConfig();
                config.Read(path);
            }
        }

        private void Initial(TimelineConfig conf, PlayMode mode, bool blend)
        {
            _time = 0;
            blending = blend;
            playMode = mode;
            config = conf;
            if (blend == false)
            {
                if (blendMixPlayable.IsValid())
                {
                    blendMixPlayable.Destroy();
                }
                if (blendPlayableOutput.IsOutputValid())
                {
                    graph.DestroyOutput(blendPlayableOutput);
                }
            }
            Build();
        }

        private void Build()
        {
            delay = 1;
            if (!graph.IsValid())
            {
                timelineRoot = new GameObject("timeline");
                graph = PlayableGraph.Create("TimelineGraph");
            }
            var tracksData = config.tracks;
            int len = tracksData.Length;
            trackTrees = new XTrack[len];
            for (int i = 0; i < len; i++)
            {
                trackTrees[i] = XTimelineFactory.GetTrack(tracksData[i], this);
                if (i >= 0 && i == config.skillHostTrack)
                {
                    SkillHostTrack = trackTrees[i];
                }
            }
            prev = 0;
            if (graph.IsValid())
            {
                graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
                graph.Play();
                if (Application.isPlaying)
                {
                    editMode = TimelinePlayMode.RealRunning;
                }
                else //if (!isRunningMode)
                {
                    ManualMode();
                }
            }
            _duration = RecalcuteDuration();
        }

        public AnimationPlayableOutput blendPlayableOutput { get; set; }
        public AnimationMixerPlayable blendMixPlayable { get; set; }

        public void BlendTo(string path)
        {
            var track = SkillHostTrack as XAnimationTrack;
            blendPlayableOutput = track.playableOutput;
            blendMixPlayable = track.mixPlayable;
            Dispose(true);
            ReadConf(path);
            if (config != null)
            {
                Initial(config, PlayMode.Skill, true);
            }
            SetPlaying(true);
        }

        public void ManualMode()
        {
            graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
            if (graph.IsPlaying())
            {
                graph.Stop();
            }
        }

#if UNITY_EDITOR
        public void EditorCheckPlay()
        {
            graph.Play();
            if (!isRunningMode)
            {
                ManualMode();
            }
        }
#endif

        public bool IsHostTrack(XTrack track)
        {
            return blending && track.ID == config.skillHostTrack;
        }

        public void Update()
        {
            if (playing)
            {
                float t = UnityEngine.Time.realtimeSinceStartup;
                float delta = 1.0f / frameRate;
                if (t - _last > delta)
                {
                    _time += delta;
                    if (_time > _duration)
                    {
                        playing = false;
                        _time = _duration;
                    }
                    if (Process(_time))
                    {
                        if (_time >= _duration)
                        {
                            Finish?.Invoke();
                        }
                    }
                    _last = t;
                }
            }
        }

        public void SetPlaying(bool play)
        {
            playing = play;
            if (play)
            {
                _duration = RecalcuteDuration();
                _time = Time;
                _last = UnityEngine.Time.realtimeSinceStartup;
                slow = 1.0f;
                if (Mathf.Abs(_time - _duration) < 1e-1)
                {
                    _time = 0;
                }
            }
        }

        public bool Process(float time)
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
                return true;
            }
            return false;
        }


        public void ProcessImmediately(float time)
        {
            if (trackTrees != null)
                for (int i = 0; i < trackTrees.Length; i++)
                {
                    trackTrees[i].Process(time, prev);
                }
            prev = time;
            if (graph.IsValid() && !isRunningMode)
            {
                graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
                graph.Evaluate(time);
            }
        }

        public void Dispose(bool blend = false)
        {
            if (trackTrees != null)
            {
                for (int i = 0; i < trackTrees.Length; i++)
                {
                    trackTrees[i].Dispose();
                }
                trackTrees = null;
            }
            if (timelineRoot)
            {
#if UNITY_EDITOR
                Object.DestroyImmediate(timelineRoot);
#else
                Object.Destroy(timelineRoot);
#endif
            }
            try
            {
                if (!blend && graph.IsValid())
                {
                    graph.Destroy();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
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
            _duration = dur;
            return dur;
        }

        public void ForTrackHierachy(System.Action<XTrack> cb)
        {
            if (trackTrees != null)
            {
                for (int i = 0; i < trackTrees.Length; i++)
                {
                    trackTrees[i].ForeachHierachyTrack(cb);
                }
            }
        }

        public void BindGo(GameObject go)
        {
            if (timelineRoot != null)
            {
                go.transform.parent = timelineRoot.transform;
            }
        }

        public static implicit operator bool(XTimeline timeline)
        {
            return timeline != null;
        }
    }
}
