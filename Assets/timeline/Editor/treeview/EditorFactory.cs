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
            return (EditorTrack) TypeUtilities.InitEObject(track);
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
