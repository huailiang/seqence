using System.IO;

namespace UnityEngine.Timeline.Data
{
    public enum ClipType
    {
        BoneFx = 1 << 1,
        SceneFx = 1 << 2,
        Animation = 1 << 3,
        PostProcess = 1 << 4,
    }

    public abstract class ClipData
    {
        public float start;

        public float duration;

        public abstract ClipType type { get; }

        public virtual void Write(BinaryWriter writer)
        {
            writer.Write((int) type);
            writer.Write(start);
            writer.Write(duration);
        }

        public virtual void Read(BinaryReader reader)
        {
            start = reader.ReadSingle();
            duration = reader.ReadSingle();
        }
    }


    public class SceneFxClipData : ClipData
    {
        public string prefab;
        public uint seed;
        public Vector3 pos, rot, scale;

        public override ClipType type
        {
            get { return ClipType.SceneFx; }
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(prefab);
            writer.Write(seed);
            writer.Write(pos);
            writer.Write(rot);
            writer.Write(scale);
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            prefab = reader.ReadString();
            seed = reader.ReadUInt32();
            pos = reader.ReadV3();
            rot = reader.ReadV3();
            scale = reader.ReadV3();
        }
    }

    public class BoneFxClipData : SceneFxClipData
    {
        public string bone;

        public override ClipType type
        {
            get { return ClipType.BoneFx; }
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(bone);
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            bone = reader.ReadString();
        }
    }

    public class AnimClipData : ClipData
    {
        public string anim;

        public float trim_start, trim_end;

        public bool loop;

        public override ClipType type
        {
            get { return ClipType.Animation; }
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(anim);
            writer.Write(trim_start);
            writer.Write(trim_end);
            writer.Write(loop);
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            anim = reader.ReadString();
            trim_start = reader.ReadSingle();
            trim_end = reader.ReadSingle();
            loop = reader.ReadBoolean();
        }
    }

    public class PostprocessData : ClipData
    {
        public override ClipType type
        {
            get { return ClipType.PostProcess; }
        }
    }
}
