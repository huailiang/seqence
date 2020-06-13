using System;
using System.IO;
using System.Xml.Serialization;

namespace UnityEngine.Timeline.Data
{
    [Serializable]
    public enum MarkType
    {
        Active = 1,
        Jump = 1 << 1,
        Slow = 1 << 2,
    }

    [XmlInclude(typeof(JumpMarkData))]
    [XmlInclude(typeof(SlowMarkData))]
    [XmlInclude(typeof(ActiveMarkData))]
    public abstract class MarkData
    {
        public float time;

        public abstract MarkType type { get; }

        public virtual void Write(BinaryWriter writer)
        {
            writer.Write((int) type);
            writer.Write(time);
        }

        public virtual void Read(BinaryReader reader)
        {
            time = reader.ReadSingle();
        }
    }


    [Serializable]
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

    [Serializable]
    public class ActiveMarkData : MarkData
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

    [Serializable]
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
