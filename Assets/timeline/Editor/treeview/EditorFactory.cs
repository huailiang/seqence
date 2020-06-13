using System;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    public class EditorFactory
    {
        public static EditorTrack GetTrack(XTrack track)
        {
            EditorTrack xtrack = null;
            switch (track.trackType)
            {
                case TrackType.Animation:
                    xtrack = new EditorAnimTrack();
                    break;
                case TrackType.SceneFx:
                    xtrack = new EditorSceneFxTrack();
                    break;
                case TrackType.BoneFx:
                    xtrack = new EditorBoneTrack();
                    break;
                case TrackType.PostProcess:
                    xtrack = new EditorPostprocessTrack();
                    break;
                default:
                    throw new Exception("unknow track build");
            }
            xtrack.@select = false;
            xtrack.track = track;
            return xtrack;
        }

        public static XMarker MakeMarker(Type t, float time)
        {
            var track = TimelineWindow.inst.timeline.trackTrees[0];
            return MakeMarker(t, time, track);
        }

        public static XMarker MakeMarker(Type t, float time, XTrack track)
        {
            XMarker marker = null;
            if (t == typeof(XJumpMarker))
            {
                marker = new XJumpMarker(track, new JumpMarkData() {time = time});
            }
            else if (t == typeof(XSlowMarker))
            {
                marker = new XSlowMarker(track, new SlowMarkData() {time = time});
            }
            else if (t == typeof(XActiveMark))
            {
                marker = new XActiveMark(track, new ActiveMarkData() {time = time});
            }
            else
            {
                Debug.LogError("unknown mark: " + t);
            }
            if (marker != null)
            {
                track.AddMarker(marker);
            }
            return marker;
        }
    }
}
