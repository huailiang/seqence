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
                    xTrack = SharedPool<XMarkerTrack>.Get();
                    break;
                case AssetType.Animation:
                    xTrack = SharedPool<XAnimationTrack>.Get();
                    break;
                case AssetType.BoneFx:
                    xTrack = SharedPool<XBoneFxTrack>.Get();
                    break;
                case AssetType.SceneFx:
                    xTrack = SharedPool<XSceneFxTrack>.Get();
                    break;
                case AssetType.PostProcess:
                    xTrack = SharedPool<XPostprocessTrack>.Get();
                    break;
                case AssetType.Transform:
                    xTrack = SharedPool<XTransformTrack>.Get();
                    break;
                case AssetType.LogicValue:
                    xTrack = SharedPool<XLogicTrack>.Get();
                    break;
                case AssetType.Group:
                    xTrack = SharedPool<XGroupTrack>.Get();
                    break;
                default:
                    Debug.LogError("unknown track " + data.type);
                    break;
            }
            if (xTrack)
            {
                xTrack.data = data;
                xTrack.timeline = tl;
                if (parent)
                {
                    xTrack.parent = parent;
                }
                xTrack.PostCreate();
            }
            return xTrack;
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
                    marker = SharedPool<XActiveMark>.Get();
                    break;
                case MarkType.Jump:
                    marker = SharedPool<XJumpMarker>.Get();
                    break;
                case MarkType.Slow:
                    marker = SharedPool<XSlowMarker>.Get();
                    break;
            }
            if (marker != null)
            {
                marker.Initial(track, data);
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
