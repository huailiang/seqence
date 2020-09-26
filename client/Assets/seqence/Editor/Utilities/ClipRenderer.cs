using UnityEngine;

namespace UnityEditor.Seqence
{
    static class ClipRenderer
    {
        static Mesh s_Quad;
        static Material s_BlendMaterial;
        static readonly Vector3[] s_Vertices = new Vector3[4];

        static readonly Vector2[] s_UVs =
        {
            new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0f), new Vector2(0f, 0f)
        };

        private static readonly int s_ManualTex2Srgb = Shader.PropertyToID("_ManualTex2SRGB");
        private static readonly int s_Color = Shader.PropertyToID("_Color");
        private static readonly int s_MainTex = Shader.PropertyToID("_MainTex");
        private static readonly int s_MaskTex = Shader.PropertyToID("_MaskTex");

        static void Initialize()
        {
            if (s_Quad == null)
            {
                s_Quad = new Mesh();
                s_Quad.hideFlags |= HideFlags.DontSave;
                s_Quad.name = "TimelineQuadMesh";

                var vertices = new Vector3[4] {Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero};
                var triangles = new int[6] {0, 1, 2, 0, 2, 3};
                var colors = new Color32[4] {Color.white, Color.white, Color.white, Color.white};

                s_Quad.vertices = vertices;
                s_Quad.uv = s_UVs;
                s_Quad.colors32 = colors;
                s_Quad.SetIndices(triangles, MeshTopology.Triangles, 0);
            }

            if (s_BlendMaterial == null)
            {
                var shader = (Shader) EditorGUIUtility.LoadRequired("Editors/TimelineWindow/DrawBlendShader.shader");
                s_BlendMaterial = new Material(shader);
            }
        }

        public static void RenderTexture(Rect r, Texture mainTex, Texture mask, Color color)
        {
            Initialize();

            s_Vertices[0] = new Vector3(r.xMin, r.yMin, 0);
            s_Vertices[1] = new Vector3(r.xMax, r.yMin, 0);
            s_Vertices[2] = new Vector3(r.xMax, r.yMax, 0);
            s_Vertices[3] = new Vector3(r.xMin, r.yMax, 0);
            s_Quad.vertices = s_Vertices;

            s_BlendMaterial.SetTexture(s_MainTex, mainTex);
            s_BlendMaterial.SetTexture(s_MaskTex, mask);
            
            s_BlendMaterial.SetFloat(s_ManualTex2Srgb,
                QualitySettings.activeColorSpace == ColorSpace.Linear ? 1.0f : 0.0f);
            s_BlendMaterial.SetColor(s_Color,
                (QualitySettings.activeColorSpace == ColorSpace.Linear) ? color.gamma : color);
            if (s_BlendMaterial.SetPass(0))
            {
                UnityEngine.Graphics.DrawMeshNow(s_Quad, Handles.matrix);
            }
        }
    }
}
