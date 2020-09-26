using UnityEngine;

namespace UnityEditor.Seqence
{
    public partial class EditorClip
    {
        private void MixProcessor()
        {
            var clips = track.eClips;
            foreach (var c in clips)
            {
                if (c.clip != this.clip)
                {
                    if (IsInRange(c.clip, clip.start))
                    {
                        var r = rect;
                        r.width = c.rect.x + c.rect.width - rect.x;
                        ProcesMixIn(r);
                    }
                    if (IsInRange(c.clip, clip.end))
                    {
                        var r = rect;
                        r.x = c.rect.x;
                        r.width = rect.x + rect.width - r.x;
                        ProcesMixOut(r);
                    }
                }
            }
        }

        private void ProcesMixIn(Rect mixInRect)
        {
            if (ValidRange(mixInRect) && mixInRect.width > 0)
            {
                var clipStyle = SeqenceStyle.timelineClip;
                var texture = clipStyle.normal.background;
                ClipRenderer.RenderTexture(mixInRect, texture, SeqenceStyle.blendMixIn.normal.background, Color.black);

                // Graphics.DrawLineAA(2.5f, new Vector3(mixInRect.xMin, mixInRect.yMax - 1f, 0),
                //     new Vector3(mixInRect.xMax, mixInRect.yMin + 1f, 0), Color.white);
            }
        }

        private void ProcesMixOut(Rect mixOutRect)
        {
            if (ValidRange(mixOutRect) && mixOutRect.width > 0)
            {
                var clipStyle = SeqenceStyle.timelineClip;
                var texture = clipStyle.normal.background;
                ClipRenderer.RenderTexture(mixOutRect, texture, SeqenceStyle.blendMixOut.normal.background,
                    Color.black);

                // Graphics.DrawLineAA(2.5f, new Vector3(mixOutRect.xMin, mixOutRect.yMax - 1f, 0),
                //     new Vector3(mixOutRect.xMax, mixOutRect.yMin + 1f, 0), Color.white);
            }
        }
    }
}
