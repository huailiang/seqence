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
                case AssetType.Marker:
                    xTrack = new XMarkerTrack(tl, data);
                    break;
                case AssetType.Animation:
                    xTrack = new XAnimationTrack(tl, data as BindTrackData);
                    break;
                case AssetType.BoneFx:
                    xTrack = new XBoneFxTrack(tl, data);
                    break;
                case AssetType.SceneFx:
                    xTrack = new XSceneFxTrack(tl, data);
                    break;
                case AssetType.PostProcess:
                    xTrack = new XPostprocessTrack(tl, data);
                    break;
                case AssetType.Transform:
                    xTrack = new XTransformTrack(tl, data);
                    break;
                case AssetType.LogicValue:
                    xTrack = new XLogicValueTrack(tl, data);
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
            else if (type == typeof(XLogicValueTrack))
            {
                data = new TrackData();
                data.type = AssetType.LogicValue;
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

        public static ClipData CreateClipData(AssetType type)
        {
            ClipData clip = null;
            switch (type)
            {
                case AssetType.Animation:
                    clip = new AnimClipData();
                    break;
                case AssetType.BoneFx:
                    clip = new BoneFxClipData();
                    break;
                case AssetType.PostProcess:
                    clip = new PostprocessData();
                    break;
                case AssetType.SceneFx:
                    clip = new SceneFxClipData();
                    break;
                case AssetType.LogicValue:
                    clip =new LogicClipData();
                    break;
                default:
                    Debug.Log("unknown clip " + type);
                    break;
            }
            return clip;
        }

        public static ClipData CreateClipData(BinaryReader reader)
        {
            AssetType type = (AssetType) reader.ReadInt32();
            var clip = CreateClipData(type);
            clip.Read(reader);
            return clip;
        }
    }
}
