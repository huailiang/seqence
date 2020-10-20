using System.Collections.Generic;
using UnityEngine;

public class ClipRange
{
    public float start, end;

    public bool Intersect(ClipRange other)
    {
        bool b1 = (start - other.start) * (end - other.start) < 0;
        bool b2 = (start - other.end) * (end - other.end) < 0;
        if (b1 & b2) return false;
        return b1 | b2;
    }

    public bool IsIn(ClipRange other)
    {
        return other.start < start && other.end > end;
    }

    public bool IsOuter(ClipRange other)
    {
        return other.start > start & other.end < end;
    }

    public bool IsSeprate(ClipRange other)
    {
        return other.end < start | other.start > end;
    }

    public bool Merge(ClipRange other)
    {
        if (Intersect(other))
        {
            start = Mathf.Min(start, other.start);
            end = Mathf.Max(end, other.end);
            return true;
        }
        if (IsIn(other) || IsOuter(other))
        {
            start = Mathf.Min(start, other.start);
            end = Mathf.Max(end, other.end);
            return true;
        }
        return false;
    }

    public static void MergeClipRange(List<ClipRange> r1, List<ClipRange> r2)
    {
        foreach (var it in r2)
        {
            bool merge = false;
            foreach (var r in r1)
            {
                if (!it.IsSeprate(r))
                {
                    merge = true;
                    r.Merge(it);
                    break;
                }
            }
            if (!merge)
            {
                r1.Add(it);
            }
        }
    }
}