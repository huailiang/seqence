using System;
using System.IO;
using System.Xml.Serialization;

namespace UnityEngine.Timeline.Data
{

    public enum LogicType
    {
        HP,
        SP,
        BeHit,
        Attack,
        MAX  //put MAX at last
    }

    public enum AttackShape
    {
        Ring,
        Sector,
        Rect,
    }

    [Serializable]
    [XmlInclude(typeof(AnimClipData))]
    [XmlInclude(typeof(SceneFxClipData))]
    [XmlInclude(typeof(BoneFxClipData))]
    [XmlInclude(typeof(PostprocessData))]
    [XmlInclude(typeof(LogicClipData))]
    public abstract class ClipData
    {
        public float start;

        public float duration;

        public abstract AssetType type { get; }

        public void BuildData(IClip c)
        {
            this.start = c.start;
            this.duration = c.duration;
        }

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

    [Serializable]
    public class SceneFxClipData : ClipData
    {
        public string prefab;
        public uint seed;
        public Vector3 pos, rot, scale;

        public override AssetType type
        {
            get { return AssetType.SceneFx; }
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

    [Serializable]
    public class BoneFxClipData : SceneFxClipData
    {
        public string bone;

        public override AssetType type
        {
            get { return AssetType.BoneFx; }
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            var b = string.IsNullOrEmpty(bone) ? "" : bone;
            writer.Write(b);
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            bone = reader.ReadString();
        }
    }

    [Serializable]
    public class LogicClipData : ClipData
    {
        public override AssetType type
        {
            get { return AssetType.LogicValue; }
        }

        public LogicType[] logicType;

        public AttackShape attackShape;

        public float[] effect;

        public float attackArg;

        public float attackArg2;

        public bool showShape;

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            attackShape = (AttackShape)reader.ReadInt32();
            attackArg = reader.ReadSingle();
            attackArg2 = reader.ReadSingle();
            showShape = reader.ReadBoolean();
            int len = reader.ReadInt32();
            logicType = new LogicType[len];
            effect = new float[len];
            for (int i = 0; i < len; i++)
            {
                logicType[i] = (LogicType)reader.ReadInt32();
                effect[i] = reader.ReadSingle();
            }
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            int len = effect?.Length ?? 0;
            writer.Write((int)attackShape);
            writer.Write(attackArg);
            writer.Write(attackArg2);
            writer.Write(showShape);
            writer.Write(len);
            for (int i = 0; i < len; i++)
            {
                writer.Write((int)logicType[i]);
                writer.Write(effect[i]);
            }
        }
    }

    [Serializable]
    public class AnimClipData : ClipData
    {
        public string anim;

        public float trim_start, trim_end;

        public bool loop;

        public override AssetType type
        {
            get { return AssetType.Animation; }
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

    [Serializable]
    public class PostprocessData : ClipData
    {
        public override AssetType type
        {
            get { return AssetType.PostProcess; }
        }
    }
}
