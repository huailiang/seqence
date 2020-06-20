using System.Collections.Generic;
using System.Linq;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    /// <summary>
    /// 之所以XTrack写在这里，是因为算法效率不高，只给editor模式下使用
    /// </summary>
    public static class XTrackEx
    {
        public static List<XTimelineObject> TrackAssets(this XTrack track)
        {
            var clips = track.clips.ToList();
            var marks = track.marks.ToList();
            var childs = track.childs.ToList();
            var list = new List<XTimelineObject>();
            for (int i = 0; i < clips.Count; i++)
            {
                XTimelineObject clip = clips[i] as XTimelineObject;
                if (clip) list.Add(clip);
            }
            list.AddRange(marks);
            list.AddRange(childs);
            return list;
        }

        public static bool AddClip(this XTrack track, IClip clip)
        {
            if (track.clips == null)
            {
                track.clips = new IClip[1];
                track.clips[0] = clip;
                return true;
            }
            else
            {
                var list = track.clips.ToList();
                if (list.Contains(clip))
                {
                    return false;
                }
                else
                {
                    list.Add(clip);
                    list.Sort((x, y) => x.start.CompareTo(y.start));
                    track.clips = list.ToArray();
                    return true;
                }
            }
        }

        public static bool RmClip(this XTrack track, IClip clip)
        {
            if (track.clips != null && track.clips.Length > 0)
            {
                var list = track.clips.ToList();
                if (list.Contains(clip))
                {
                    list.Remove(clip);
                    track.clips = list.ToArray();
                    return true;
                }
            }
            return false;
        }

        public static bool AddMarker(this XTrack track, XMarker marker)
        {
            if (track.marks != null)
            {
                var list = track.marks.ToList();
                if (!list.Contains(marker))
                {
                    list.Add(marker);
                    list.Sort((x, y) => x.time.CompareTo(y.time));
                    track.marks = list.ToArray();
                    return true;
                }
            }
            else
            {
                track.marks = new XMarker[1];
                track.marks[0] = marker;
            }
            return false;
        }

        public static bool RmMarker(this XTrack track, XMarker marker)
        {
            if (track.marks != null)
            {
                var list = track.marks.ToList();
                if (list.Contains(marker))
                {
                    list.Remove(marker);
                    track.marks = list.ToArray();
                    return true;
                }
            }
            return false;
        }

        public static void SortClip(this XTrack track)
        {
            if (track.clips != null && track.clips.Length > 0)
            {
                var list = track.clips.ToList();
                list.Sort((x, y) => x.start.CompareTo(y.start));
                track.clips = list.ToArray();
            }
        }

        public static void SortMark(this XTrack track)
        {
            if (track.marks != null && track.marks.Length > 0)
            {
                var list = track.marks.ToList();
                list.Sort((x, y) => x.time.CompareTo(y.time));
                track.marks = list.ToArray();
            }
        }

        public static void AddRootTrack(this XTimeline timeline, XTrack track)
        {
            var tracks = timeline.trackTrees.ToList();
            tracks.Add(track);
            timeline.trackTrees = tracks.ToArray();
        }

        public static void AddSub(this XTrack track, XTrack sub)
        {
            if (track.childs == null)
            {
                track.childs = new XTrack[1];
                track.childs[0] = sub;
            }
            else
            {
                var tmp = new XTrack[track.childs.Length + 1];
                for (int i = 0; i < track.childs.Length; i++)
                {
                    tmp[i] = track.childs[i];
                }
                tmp[track.childs.Length] = sub;
                track.childs = tmp;
            }
        }

        public static void Remove(this XTrack track, XTimeline timeline)
        {
            if (track.parent)
            {
                var chs = track.parent.childs;
                int idx = -1;
                for (int i = 0; i < chs.Length; i++)
                {
                    if (chs[i].Equals(track))
                    {
                        idx = i;
                        break;
                    }
                }
                if (idx >= 0)
                {
                    int len = chs.Length - 1;
                    var tmp = new XTrack[len];
                    for (int i = 0; i < len; i++)
                    {
                        tmp[i] = i < idx ? chs[i] : chs[i + 1];
                    }
                    track.parent.childs = tmp;
                }
            }
            else
            {
                var list = timeline.trackTrees.ToList();
                if (list.Contains(track))
                {
                    list.Remove(track);
                    timeline.trackTrees = list.ToArray();
                }
            }
        }

        public static bool IsAllSubTrackMuted(this XTrack track)
        {
            bool ret = true;
            track.ForeachHierachyTrack((t) => { ret = ret & t.mute; });
            return ret;
        }

        public static void BuildConf(this XTimeline timeline)
        {
            var tree = timeline.trackTrees;
            if (tree != null)
            {
                int len = tree.Length;
                timeline.config.tracks = new TrackData[len];
                for (int i = 0; i < len; i++)
                {
                    timeline.config.tracks[i] = tree[i].BuildTrackData();
                }
            }
        }

        public static TrackData BuildTrackData(this XTrack track)
        {
            TrackData data = new TrackData();
            data.type = track.AssetType;

            if (track.clips != null)
            {
                data.clips = new ClipData[track.clips.Length];
                for (int i = 0; i < track.clips.Length; i++)
                {
                    data.clips[i] = track.clips[i].data;
                }
            }
            if (track.marks != null)
            {
                data.marks = new MarkData[track.marks.Length];
                for (int i = 0; i < track.marks.Length; i++)
                {
                    data.marks[i] = track.marks[i].MarkData;
                }
            }
            if (track.childs != null)
            {
                data.childs = new TrackData[track.childs.Length];
                for (int i = 0; i < data.childs.Length; i++)
                {
                    data.childs[i] = BuildTrackData(track.childs[i]);
                }
            }
            return data;
        }
    }
}
