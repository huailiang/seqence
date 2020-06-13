using System;
using System.IO;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XTimelineFactory
    {
        public static XTrack GetTrack(TrackData data, XTimeline tl)
        {
            XTrack xTrack = null;
            switch (data.type)
            {
                case TrackType.Marker:
                    xTrack = new XMarkerTrack(tl, data);
                    break;
                case TrackType.Animation:
                    xTrack = new XAnimationTrack(tl, data as BindTrackData);
                    break;
                case TrackType.BoneFx:
                    xTrack = new XBoneFxTrack(tl, data);
                    break;
                case TrackType.SceneFx:
                    xTrack = new XSceneFxTrack(tl, data);
                    break;
                case TrackType.PostProcess:
                    xTrack = new XPostprocessTrack(tl, data);
                    break;
                case TrackType.Transform:
                    xTrack = new XTransformTrack(tl, data);
                    break;
                default:
                    Debug.LogError("unknown track " + data.type);
                    break;
            }
            if (xTrack)
            {
                xTrack.OnPostBuild();
            }
            return xTrack;
        }

        public static TrackData CreateTrackData(Type type)
        {
            TrackData data = null;
            if (type == typeof(XAnimationTrack))
            {
                data = new BindTrackData();
                data.type = TrackType.Animation;
            }
            else if (type == typeof(XPostprocessTrack))
            {
                data = new TrackData();
                data.type = TrackType.PostProcess;
            }
            else if (type == typeof(XBoneFxTrack))
            {
                data = new TrackData();
                data.type = TrackType.BoneFx;
            }
            else if (type == typeof(XSceneFxTrack))
            {
                data = new TrackData();
                data.type = TrackType.SceneFx;
            }
            else if (type == typeof(XTransformTrack))
            {
                data = new TransformTrackData();
                data.type = TrackType.Transform;
            }
            else
            {
                throw new Exception("not implement trackdata for default");
            }
            return data;
        }

        public static MarkData CreateMarkData(MarkType type)
        {
            MarkData data = null;
            switch (type)
            {
                case MarkType.Active:
                    data = new ActiveMarkData();
                    break;
                case MarkType.Jump:
                    data = new JumpMarkData();
                    break;
                case MarkType.Slow:
                    data = new SlowMarkData();
                    break;
                default:
                    Debug.LogError("unknowm marktype: " + type);
                    break;
            }
            return data;
        }

        public static MarkData CreateMarkData(BinaryReader reader)
        {
            MarkType type = (MarkType) reader.ReadInt32();
            var mark = CreateMarkData(type);
            mark.Read(reader);
            return mark;
        }

        public static ClipData CreateClipData(ClipType type)
        {
            ClipData clip = null;
            switch (type)
            {
                case ClipType.Animation:
                    clip = new AnimClipData();
                    break;
                case ClipType.BoneFx:
                    clip = new BoneFxClipData();
                    break;
                case ClipType.PostProcess:
                    clip = new PostprocessData();
                    break;
                case ClipType.SceneFx:
                    clip = new SceneFxClipData();
                    break;
                default:
                    Debug.Log("unknown clip " + type);
                    break;
            }
            return clip;
        }

        public static ClipData CreateClipData(BinaryReader reader)
        {
            ClipType type = (ClipType) reader.ReadInt32();
            var clip = CreateClipData(type);
            clip.Read(reader);
            return clip;
        }
    }
}
