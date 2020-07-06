using System.Collections.Generic;
using System.Linq;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    // 之所以XTrack写在这里，是因为算法效率不高，只给editor模式下使用
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
                if (clip != null) list.Add(clip);
            }
            list.AddRange(marks);
            list.AddRange(childs);
            return list;
        }

        public static bool AddClip(this XTrack track, IClip clip, ClipData data)
        {
            if (track.clips == null)
            {
                track.clips = new IClip[1];
                track.clips[0] = clip;
                track.data.clips = new[] {data};
                return true;
            }
            else
            {
                var list = track.clips.ToList();
                var datas = track.data.clips.ToList();
                if (list.Contains(clip))
                {
                    return false;
                }
                else
                {
                    list.Add(clip);
                    datas.Add(data);
                    list.Sort((x, y) => x.start.CompareTo(y.start));
                    datas.Sort((x, y) => x.start.CompareTo(y.start));
                    track.clips = list.ToArray();
                    track.data.clips = datas.ToArray();
                    return true;
                }
            }
        }

        public static bool RmClip(this XTrack track, IClip clip)
        {
            if (track.clips != null && track.clips.Length > 0)
            {
                var list = track.clips.ToList();
                var datas = track.data.clips.ToList();
                if (datas.Contains(clip.data))
                {
                    datas.Remove(clip.data);
                    track.data.clips = datas.ToArray();
                }
                if (list.Contains(clip))
                {
                    list.Remove(clip);
                    track.clips = list.ToArray();
                    return true;
                }
            }
            return false;
        }

        public static bool AddMarker(this XTrack track, XMarker marker, MarkData data)
        {
            if (track.marks != null)
            {
                var list = track.marks.ToList();

                if (!list.Contains(marker))
                {
                    list.Add(marker);
                    list.Sort((x, y) => x.time.CompareTo(y.time));
                    track.marks = list.ToArray();

                    var datas = track.data.marks.ToList();
                    datas.Add(data);
                    track.data.marks = datas.ToArray();
                    return true;
                }
            }
            else
            {
                track.marks = new XMarker[1];
                track.marks[0] = marker;
                track.data.marks = new[] {data};
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
                    var datas = track.data.marks.ToList();
                    datas.Remove(marker.MarkData);
                    track.data.marks = datas.ToArray();

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

                var datas = track.data.clips.ToList();
                datas.Sort((x, y) => x.start.CompareTo(y.start));
                track.data.clips = datas.ToArray();
            }
        }

        public static void SortMark(this XTrack track)
        {
            if (track.marks != null && track.marks.Length > 0)
            {
                var list = track.marks.ToList();
                list.Sort((x, y) => x.time.CompareTo(y.time));
                track.marks = list.ToArray();

                var datas = track.data.marks.ToList();
                datas.Sort((x, y) => x.time.CompareTo(y.time));
                track.data.marks = datas.ToArray();
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

        public static void RemoveChild(this XTrack track, int idx)
        {
            if (track.childs != null && track.childs.Length > idx)
            {
                var list = track.childs.ToList();
                list.RemoveAt(idx);
                track.childs = list.ToArray();
                var list2 = track.data.childs.ToList();
                list2.RemoveAt(idx);
                track.data.childs = list2.ToArray();
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
                    track.parent.RemoveChild(idx);
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

        public static void AddTrackChildData(this XTrack track, TrackData data)
        {
            if (track.data.childs == null)
            {
                track.data.childs = new[] {data};
            }
            else
            {
                var list = track.data.childs.ToList();
                list.Add(data);
                track.data.childs = list.ToArray();
            }
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
                    timeline.config.tracks[i] = tree[i].data;
                }
            }
        }
    }
}
