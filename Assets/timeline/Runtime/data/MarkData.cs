using System.IO;

namespace UnityEngine.Timeline.Data
{
    public enum MarkType
    {
        Active = 1,
        Jump = 1 << 1,
        Slow = 1 << 2,
    }

    public class MarkData
    {
        public float time;

        public virtual void Write(BinaryWriter writer)
        {
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

        public override void Write(BinaryWriter writer)
        {
            writer.Write((int) MarkType.Slow);
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


        public override void Write(BinaryWriter writer)
        {
            writer.Write((int) MarkType.Active);
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

        public override void Write(BinaryWriter writer)
        {
            writer.Write((int) MarkType.Jump);
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
