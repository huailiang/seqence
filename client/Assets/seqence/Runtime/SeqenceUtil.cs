using System.IO;

namespace UnityEngine.Seqence
{
    public class SeqenceUtil
    {

        public static readonly string chpath = "Assets/seqence/Editor/StyleSheets/CharacterInfo.asset";

        public static void Add<T>(ref T[] arr, T item) where T : new()
        {
            if (arr != null)
            {
                T[] narr = new T[arr.Length + 1];
                for (int i = 0; i < arr.Length; i++)
                {
                    narr[i] = arr[i];
                }
                narr[arr.Length] = item;
                arr = narr;
            }
            else
            {
                arr = new T[1];
                arr[0] = item;
            }
        }

        public static T[] Remv<T>(T[] arr, int idx)
        {
            if (arr.Length > idx)
            {
                T[] narr = new T[arr.Length - 1];
                for (int i = 0; i < idx; i++)
                {
                    narr[i] = arr[i];
                }
                for (int i = idx + 1; i < arr.Length; i++)
                {
                    narr[i - 1] = arr[i];
                }
                return narr;
            }
            else
            {
                return arr;
            }
        }

    }


    public static class BinaryWriterEx
    {
        public static void Write(this BinaryWriter writer, Vector3 v3)
        {
            writer.Write(v3.x);
            writer.Write(v3.y);
            writer.Write(v3.z);
        }

        public static void Write(this BinaryWriter writer, Vector4 v4)
        {
            writer.Write(v4.x);
            writer.Write(v4.y);
            writer.Write(v4.z);
            writer.Write(v4.w);
        }
    }

    public static class BinaryReaderEx
    {
        public static Vector3 ReadV3(this BinaryReader reader)
        {
            Vector3 v3;
            v3.x = reader.ReadSingle();
            v3.y = reader.ReadSingle();
            v3.z = reader.ReadSingle();
            return v3;
        }

        public static Vector4 ReadV4(this BinaryReader reader)
        {
            Vector4 v4;
            v4.x = reader.ReadSingle();
            v4.y = reader.ReadSingle();
            v4.z = reader.ReadSingle();
            v4.w = reader.ReadSingle();
            return v4;
        }
    }
}