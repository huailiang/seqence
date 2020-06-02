using UnityEngine.Timeline;

public interface IMixClip
{
    bool connect { get; set; }
    float start { get; set; }
    float duration { get; set; }
    IClip blendA { get; set; }
    IClip blendB { get; set; }
    bool IsIn(float time);
}

public sealed class XMixClip<T> : IMixClip where T : XTrack
{
    public bool connect { get; set; }
    public float start { get; set; }
    public float duration { get; set; }
    public IClip blendA { get; set; }
    public IClip blendB { get; set; }

    public XMixClip(float start, float duration, IClip clip1, IClip clip2)
    {
        this.start = start;
        this.duration = duration;
        blendA = clip1;
        blendB = clip2;
        connect = false;
    }

    public bool IsIn(float time)
    {
        return time >= start && time < start + duration;
    }
}
