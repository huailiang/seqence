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
            MarkData data = null;
            if (t == typeof(XJumpMarker))
            {
                data = new JumpMarkData() {time = time};
                marker = new XJumpMarker(track, (JumpMarkData) data);
            }
            else if (t == typeof(XSlowMarker))
            {
                data = new SlowMarkData() {time = time};
                marker = new XSlowMarker(track, (SlowMarkData) data);
            }
            else if (t == typeof(XActiveMark))
            {
                data = new ActiveMarkData() {time = time};
                marker = new XActiveMark(track, (ActiveMarkData) data);
            }
            else
            {
                Debug.LogError("unknown mark: " + t);
            }
            if (marker != null)
            {
                track.AddMarker(marker, data);
            }
            return marker;
        }
    }
}
