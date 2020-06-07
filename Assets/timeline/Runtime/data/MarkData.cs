using System.IO;

namespace UnityEngine.Timeline.Data
{
    public enum MarkType
    {
        Active = 1,
        Jump = 1 << 1,
        Slow = 1 << 2,
    }

    public abstract class MarkData
    {
        public float time;

        public abstract MarkType type { get; }

        public virtual void Write(BinaryWriter writer)
        {
            writer.Write((int)type);
            writer.Write(time);
        }

        public virtual void Read(BinaryReader reader)
        {
            time = reader.ReadSingle();
        }
    }


    public class SlowMarkData : MarkData
    {
        public float slowRate;

        public override MarkType type
        {
            get { return MarkType.Slow; }
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(slowRate);
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            slowRate = reader.ReadSingle();
        }
    }

    public class ActiveData : MarkData
    {
        public bool active;

        public override MarkType type
        {
            get { return MarkType.Active; }
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(active);
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            active = reader.ReadBoolean();
        }
    }

    public class JumpMarkData : MarkData
    {
        public float jump;

        public override MarkType type
        {
            get { return MarkType.Jump; }
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(jump);
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            jump = reader.ReadSingle();
        }
    }
}
