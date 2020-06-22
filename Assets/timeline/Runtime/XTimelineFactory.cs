using System;
using System.IO;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XTimelineFactory
    {
        public static XTrack GetTrack(TrackData data, XTimeline tl, XTrack parent = null)
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
                case AssetType.Group:
                    xTrack = new XGroupTrack(tl, data);
                    break;
                default:
                    Debug.LogError("unknown track " + data.type);
                    break;
            }
            if (xTrack)
            {
                if (parent)
                {
                    xTrack.parent = parent;
                }
                xTrack.OnPostBuild();
            }
            return xTrack;
        }

        private static AssetType MapTrackType(Type type)
        {
            AssetType ret = AssetType.None;
            if (type == typeof(XAnimationTrack))
            {
                ret = AssetType.Animation;
            }
            else if (type == typeof(XPostprocessTrack))
            {
            }
            return ret;
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

        public static XMarker GetMarker(XTrack track, MarkData data)
        {
            XMarker marker = null;
            switch (data.type)
            {
                case MarkType.Active:
                    marker = new XActiveMark(track, data);
                    break;
                case MarkType.Jump:
                    marker = new XJumpMarker(track, data);
                    break;
                case MarkType.Slow:
                    marker = new XSlowMarker(track, data);
                    break;
            }
            return marker;
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
                    clip = new LogicClipData();
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
