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
                data = new JumpMarkData() { time = time };
                marker = new XJumpMarker(track, (JumpMarkData)data);
            }
            else if (t == typeof(XSlowMarker))
            {
                data = new SlowMarkData() { time = time, slowRate = 0.5f };
                marker = new XSlowMarker(track, (SlowMarkData)data);
            }
            else if (t == typeof(XActiveMark))
            {
                data = new ActiveMarkData() { time = time };
                marker = new XActiveMark(track, (ActiveMarkData)data);
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


        public static void GetTrackByDataType(Type type, XTimeline timeline, XTrack parent,
            Action<XTrack, TrackData, object> cb)
        {
            TrackData data = CreateTrackData(type);
            if (data != null)
            {
                if (type == typeof(XAnimationTrack))
                {
                    CharacterWindow.ShowWindow(ch =>
                    {
                        if (ch != null)
                        {
                            var bd = data as AnimationTrackData;
                            bd.prefab = ch.prefab;
                            bd.roleid = ch.id;
                            cb(XTimelineFactory.GetTrack(data, timeline, parent), data, ch);
                        }
                        else
                        {
                            cb(null, data, null);
                        }
                    });
                }
                else
                {
                    cb(XTimelineFactory.GetTrack(data, timeline, parent), data, null);
                }
            }
            else
                cb(null, null, null);
        }

        private static TrackData CreateTrackData(Type type)
        {
            TrackData data = null;
            if (type == typeof(XAnimationTrack))
            {
                data = new AnimationTrackData();
                data.type = AssetType.Animation;
            }
            else if (type == typeof(XPostprocessTrack))
            {
                data = new TrackData();
                data.type = AssetType.PostProcess;
            }
            else if (type == typeof(XBoneFxTrack))
            {
                data = new TrackData();
                data.type = AssetType.BoneFx;
            }
            else if (type == typeof(XSceneFxTrack))
            {
                data = new TrackData();
                data.type = AssetType.SceneFx;
            }
            else if (type == typeof(XTransformTrack))
            {
                data = new TransformTrackData();
                data.type = AssetType.Transform;
            }
            else if (type == typeof(XLogicTrack))
            {
                data = new TrackData();
                data.type = AssetType.LogicValue;
            }
            else if (type == typeof(XGroupTrack))
            {
                data = new GroupTrackData();
                data.type = AssetType.Group;
            }
            else
            {
                throw new Exception("not implement trackdata for default");
            }
            return data;
        }
    }
}
