using UnityEngine.Timeline;

public interface IMixClip
{
    float start { get; set; }
    float duration { get; set; }
    IClip blendA { get; set; }
    IClip blendB { get; set; }
    void Update(float time, float prev);
}

public sealed class XMixClip<T> : IMixClip where T : XTrack
{
    public float start { get; set; }
    public float duration { get; set; }
    public IClip blendA { get; set; }
    public IClip blendB { get; set; }

    public XMixClip(float start, float duration,IClip clip1,IClip clip2)
    {
        this.start = start;
        this.duration = duration;
        blendA = clip1;
        blendB = clip2;
    }

    public void Update(float time, float prev)
    {
        float offset = time - start;
        if (offset >= 0)
        {
            float w = offset / duration;
            OnMix(w);
        }
    }

    private void OnMix(float weight)
    {
        
    }
}
