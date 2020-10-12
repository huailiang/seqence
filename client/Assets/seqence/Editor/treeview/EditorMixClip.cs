using UnityEngine;

namespace UnityEditor.Seqence
{
    
    public partial class EditorClip
    {
        Vector3[] s_BlendVertices = new Vector3[3];
        readonly Color inColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);

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
                Color backgroundColor = new Color(0.4f, 0.4f, 0.4f, 1.0f);
                

                EditorGUI.DrawRect(mixInRect, backgroundColor);
                s_BlendVertices[0] = new Vector3(mixInRect.xMin, mixInRect.yMax);
                s_BlendVertices[1] = new Vector3(mixInRect.xMax, mixInRect.yMax);
                s_BlendVertices[2] = new Vector3(mixInRect.xMax, mixInRect.yMin);
                Graphics.DrawPolygonAA(inColor, s_BlendVertices);
            }
        }

        private void ProcesMixOut(Rect mixOutRect)
        {
            if (ValidRange(mixOutRect) && mixOutRect.width > 0)
            {
                s_BlendVertices[0] = new Vector3(mixOutRect.xMin, mixOutRect.yMin);
                s_BlendVertices[1] = new Vector3(mixOutRect.xMax, mixOutRect.yMax);
                s_BlendVertices[2] = new Vector3(mixOutRect.xMax, mixOutRect.yMin);
                Graphics.DrawPolygonAA(inColor, s_BlendVertices);
            }
        }

    }
}
