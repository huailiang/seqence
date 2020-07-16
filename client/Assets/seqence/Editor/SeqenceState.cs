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

    public class SeqenceState
    {
        public SeqenceState(SeqenceWindow win)
        {
            window = win;
            Initial();
        }

        public XSeqence seqence;
        public WrapMode mode;
        private string name;
        public SeqenceWindow window;
        public AssetConfig config;
        public string path;

        public bool showMarkerHeader { get; set; }

        public int frameRate
        {
            get { return XSeqence.frameRate; }
        }

        public bool playing
        {
            get { return seqence?.playing ?? false; }
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
                seqence?.Dispose();
                seqence = null;
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
            if (seqence == null && Application.isPlaying)
            {
                var run = Object.FindObjectOfType<RuntimeSeqence>();
                if (run) seqence = run.seqence;
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
            seqence = new XSeqence(xconf, mode);
            seqence.Finish = OnPlayFinish;
            AddRuntime();
            seqence.Time = 1.0f;
            seqence.editMode = SeqencePlayMode.EditorRun;
        }

        public void Open(string path, PlayMode mode)
        {
            Dispose();
            this.path = path;
            seqence = new XSeqence(path, mode);
            seqence.Finish = OnPlayFinish;
            AddRuntime();
            float dur = seqence.RecalcuteDuration();
            window.SetTimeRange(0, dur * 1.5f);
        }

        public void Dispose()
        {
            path = string.Empty;
            CleanEnv();
            RebuildInspector();
            seqence?.Dispose();
            seqence = null;
            if (window != null)
            {
                window.Dispose();
            }
            XResources.Clean();
        }

        private void AddRuntime()
        {
            var go = GameObject.Find("timeline");
            if (go)
            {
                var runtime = go.GetComponent<RuntimeSeqence>();
                if (runtime == null) runtime = go.AddComponent<RuntimeSeqence>();
                runtime.path = this.path;
                runtime.seqence = seqence;
            }
        }


        public void Save()
        {
            if (!string.IsNullOrEmpty(path))
            {
                seqence.BuildConf();
                seqence.config.Write(path);
                var p = path.Replace(".bytes", ".xml");
                seqence.config.WriteXml(p);
                RebuildInspector();
            }
            else
            {
                EditorUtility.DisplayDialog("warn", "save path is null", "ok");
            }
        }

        private void RebuildInspector()
        {
            var insp = SeqenceInspector.inst;
            if (insp)
            {
                insp.OnRebuild();
            }
        }

        public void NextFrame()
        {
            if (seqence)
            {
                float time = seqence.Time + 1.0f / frameRate;
                seqence.ProcessTo(time);
            }
        }

        public void PrevFrame()
        {
            if (seqence)
            {
                float time = seqence.Time - 1.0f / frameRate;
                seqence.ProcessTo(time);
            }
        }

        public void FrameStart()
        {
            seqence.SetPlaying(false);
            seqence?.ProcessTo(0);
        }

        public void FrameEnd()
        {
            if (seqence)
            {
                seqence.SetPlaying(false);
                float end = seqence.RecalcuteDuration();
                seqence.ProcessTo(end);
            }
        }


        public void Update()
        {
            seqence?.Update();
            if (playing)
            {
                window.Repaint();
            }
        }

        private void OnPlayFinish()
        {
            SetPlaying(mode == WrapMode.Loop);
            window.Repaint();
        }

        public void SetPlaying(bool play)
        {
            seqence.SetPlaying(play);
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
