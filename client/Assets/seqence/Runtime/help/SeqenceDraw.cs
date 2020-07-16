using System.Collections.Generic;

namespace UnityEngine.Timeline
{
    public class SeqenceDraw
    {

        private GameObject sharedObj;
        private MeshFilter sharedFilter;
        private MeshRenderer sharedRender;
        private static Material sharedMat;


        private void SetupIfNoExist()
        {
            if (sharedObj == null)
            {
                sharedObj = new GameObject();

                sharedFilter = sharedObj.AddComponent<MeshFilter>();
                sharedRender = sharedObj.AddComponent<MeshRenderer>();
            }
            if (sharedMat == null)
            {
                var shader = Shader.Find("Unlit/Attack");
                sharedMat = new Material(shader);
            }
        }

        public void Destroy()
        {
            if (sharedObj != null)
            {
#if UNITY_EDITOR
                Object.DestroyImmediate(sharedObj);
#else
                Object.Destroy(sharedObj);
#endif
            }
        }

        public void Clean()
        {
            if (sharedFilter?.sharedMesh != null)
            {
#if UNITY_EDITOR
                Object.DestroyImmediate(sharedFilter.sharedMesh);
#else
                Object.Destroy(sharedFilter.sharedMesh);
#endif
                sharedFilter.sharedMesh = null;
            }
            if (sharedRender)
                sharedRender.sharedMaterial = null;
        }

        /// <summary>
        /// 绘制实心圆形
        /// </summary>
        public GameObject DrawCircleSolid(Transform t, float radius)
        {
            Vector3 forward = t.forward;
            Vector3 center = t.position;
            return DrawCircleSolid(center, forward, radius);
        }

        public GameObject DrawCircleSolid(Vector3 center, Vector3 forward, float radius)
        {
            int pointAmount = 100;
            float eachAngle = 360f / pointAmount;
            center += new Vector3(0, 0.01f, 0);
            List<Vector3> vertices = new List<Vector3>();
            for (int i = 0; i < pointAmount; i++)
            {
                Vector3 pos = Quaternion.Euler(0f, eachAngle * i, 0f) * forward * radius + center;
                vertices.Add(pos);
            }
            CreateMesh(vertices, "circle");
            return sharedObj;
        }


        /// <summary>
        /// 绘制实心长方形
        /// </summary>
        public GameObject DrawRectangleSolid(Transform t, float length, float width)
        {
            List<Vector3> vertices = new List<Vector3>();
            Vector3 center = t.position;
            center += new Vector3(0, 0.01f, 0);
            vertices.Add(center - t.right * width);
            vertices.Add(center - t.right * width + t.forward * length);
            vertices.Add(center + t.right * width + t.forward * length);
            vertices.Add(center + t.right * width);

            CreateMesh(vertices, "rect");
            return sharedObj;
        }


        /// <summary>
        /// 绘制扇形区域
        /// </summary>
        public GameObject DrawSectorSolid(Transform t, float angle, float radius)
        {
            int pointAmmount = 100;
            float eachAngle = angle / pointAmmount;

            Vector3 forward = t.forward;
            List<Vector3> vertices = new List<Vector3>();

            Vector3 center = t.position;
            center += new Vector3(0, 0.01f, 0);
            vertices.Add(center);
            for (int i = 0; i < pointAmmount; i++)
            {
                Vector3 pos = Quaternion.Euler(0f, -angle / 2 + eachAngle * (i - 1), 0f) * forward * radius + center;
                vertices.Add(pos);
            }
            CreateMesh(vertices, "sector");
            return sharedObj;
        }


        public void CreateMesh(List<Vector3> vertices, string name)
        {
            int[] triangles;
            Mesh mesh = new Mesh();
            int triangleAmount = vertices.Count - 2;
            triangles = new int[3 * triangleAmount];

            //根据三角形的个数，来计算绘制三角形的顶点顺序
            //顺序必须为顺时针或者逆时针
            for (int i = 0; i < triangleAmount; i++)
            {
                triangles[3 * i] = 0;
                triangles[3 * i + 1] = i + 1;
                triangles[3 * i + 2] = i + 2;
            }
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles;

            SetupIfNoExist();

#if UNITY_EDITOR
            sharedObj.name = name;
#endif

            sharedFilter.mesh = mesh;
            sharedRender.sharedMaterial = sharedMat;
            sharedMat.color = Color.red;
        }

    }
}