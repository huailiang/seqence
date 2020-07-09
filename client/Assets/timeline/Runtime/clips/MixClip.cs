using UnityEngine.Timeline;


public sealed class MixClip :  ISharedObject<MixClip>
{
    public bool connect { get; set; }
    public float start { get; set; }
    public float duration { get; set; }
    public IClip blendA { get; set; }
    public IClip blendB { get; set; }

    public MixClip next { get; set; }
    
    public void Initial(float start, float duration, IClip clip1, IClip clip2)
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

    public void Dispose()
    {
        connect = false;
        blendA = null;
        blendB = null;
    }
}
