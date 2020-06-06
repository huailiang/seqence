using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    /// <summary>
    /// 之所以XTrack写在这里，是因为算法效率不高，只给editor模式下使用
    /// </summary>
    public static class XTrackEx
    {
        public static bool AddClip(this XTrack track, IClip clip)
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

        public static bool RmClip(this XTrack track, IClip clip)
        {
            var list = track.clips.ToList();
            if (list.Contains(clip))
            {
                list.Remove(clip);
                return true;
            }
            return false;
        }

        public static bool AddMarker(this XTrack track, XMarker marker)
        {
            var list = track.marks.ToList();
            if (list.Contains(marker))
            {
                list.Add(marker);
                list.Sort((x, y) => x.time.CompareTo(y.time));
                track.marks = list.ToArray();
                return true;
            }
            return false;
        }

        public static bool RmMarker(this XTrack track, XMarker marker)
        {
            var list = track.marks.ToList();
            if (list.Contains(marker))
            {
                list.Remove(marker);
                return true;
            }
            return false;
        }

        public static void SortClip(this XTrack track)
        {
            var list = track.clips.ToList();
            list.Sort((x, y) => x.start.CompareTo(y.start));
            track.clips = list.ToArray();
        }

        public static void SortMark(this XTrack track)
        {
            var list = track.marks.ToList();
            list.Sort((x, y) => x.time.CompareTo(y.time));
            track.marks = list.ToArray();
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

        private static void Remv(this XTrack track)
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
        }

        public static bool IsAllSubTrackMuted(this XTrack track)
        {
            bool ret = true;
            track.ForeachHierachyTrack((t) => { ret = ret & t.mute; });
            return ret;
        }
    }
}
