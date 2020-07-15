using System;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline.Data;
#if UNITY_2019_3_OR_NEWER
using UnityEngine.Animations;
#else
using UnityEngine.Experimental.Animations;
#endif

namespace UnityEngine.Timeline
{
    public enum TimelinePlayMode
    {
        EditorPause,
        EditorRun,
        RealRunning,
    }

    public enum PlayMode { Plot, Skill }

    public class XTimelineObject { }

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
                    ProcessTo(value);
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
            blending = false;
            Initial(conf, mode);
        }

        public XTimeline(string path, PlayMode mode, Animator ator = null)
        {
            blending = false;
            ReadConf(path);
            if (mode == PlayMode.Skill)
            {
                hostAnimator = ator;
            }
            if (config != null)
            {
                Initial(config, mode);
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

        private void Initial(TimelineConfig conf, PlayMode mode)
        {
            _time = 0;
            playMode = mode;
            config = conf;
            if (!blending)
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
                if (Application.isPlaying)
                {
                    graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
                    graph.Play();
                    editMode = TimelinePlayMode.RealRunning;
                }
            }
            _duration = RecalcuteDuration();
        }

        public Animator hostAnimator { get; set; }
        public AnimationPlayableOutput blendPlayableOutput { get; set; }
        public AnimationScriptPlayable blendMixPlayable { get; set; }
        public MixerJob mixJob { get; set; }


        // Only skill mode worked
        public void BlendTo(string path)
        {
            var track = SkillHostTrack as XAnimationTrack;
            AnimClipData data = null;
            if (track)
            {
                blendPlayableOutput = track.playableOutput;
                blendMixPlayable = track.mixPlayable;
                mixJob = track.mixJob;
                var clip = track.GetPlayingPlayable(out var tick) as XAnimationClip;
                if (clip != null)
                {
                    var p = clip.playable;
                    data = (AnimClipData)clip.data;
                    data.trim_start = tick;
                    data.duration = Mathf.Min(tick + 0.1f, data.duration);
                    data.start = 0.01f;
                }
            }
            blending = true;
            Dispose(true);
            ReadConf(path);
            if (config != null)
            {
                if (config.skillHostTrack <= 0)
                {
                    Debug.LogError("not config skill host " + path);
                    return;
                }
                if (data != null)
                {
                    var clips = config.tracks[config.skillHostTrack].clips;
                    var nc = new ClipData[clips.Length + 1];
                    nc[0] = data;
                    for (int i = 1; i < clips.Length + 1; i++)
                    {
                        nc[i] = clips[i - 1];
                    }
                    config.tracks[config.skillHostTrack].clips = nc;
                }
                Initial(config, PlayMode.Skill);
            }
            SetPlaying(true);
        }

        public bool IsHostTrack(XTrack track)
        {
            if (playMode == PlayMode.Skill)
            {
                int idx = config.skillHostTrack;
                var tracks = config.tracks;
                return tracks.Length > idx &&
                    tracks[idx] == track.data;
            }
            return false;
        }

        public void Update()
        {
            if (playing)
            {
                float t = UnityEngine.Time.realtimeSinceStartup;
                float delta = 1.0f / frameRate * slow;
                if (t - _last > delta)
                {
                    _time += delta;
                    if (_time > _duration)
                    {
                        playing = false;
                        _time = _duration;
                    }
                    ProcessTo(_time);
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
        
        public void ProcessTo(float time)
        {
            if (trackTrees != null)
                for (int i = 0; i < trackTrees.Length; i++)
                {
                    trackTrees[i].Process(time, prev);
                }
            prev = time;
            if (graph.IsValid())
            {
                if (!Application.isPlaying)
                {
                    if (graph.IsPlaying()) graph.Stop();
                    graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
                    if (graph.GetOutputCount() > 0)
                    {
                        graph.Evaluate(time);
                    }
                }
            }
        }

        public void JumpTo(float t)
        {
            _time = t;
            ProcessTo(t);
        }

        public void Dispose(bool blend = false)
        {
            if (trackTrees != null)
            {
                for (int i = 0; i < trackTrees.Length; i++)
                {
                    trackTrees[i].OnDestroy();
                }
                trackTrees = null;
            }
            if (timelineRoot && !blend)
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
