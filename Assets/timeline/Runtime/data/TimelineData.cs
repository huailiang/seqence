using System.IO;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UnityEngine.Timeline.Data
{
    public enum TrackType
    {
        Marker = 1,
        BoneFx = 1 << 1,
        SceneFx = 1 << 2,
        Animation = 1 << 3,
        PostProcess = 1 << 4,
    }

    public class TrackData
    {
        public ClipData[] clips;
        public MarkData[] marks;
        public TrackData[] childs;
        public TrackType type;

        public TrackData(TrackType t)
        {
            type = t;
        }

        public virtual void Write(BinaryWriter writer)
        {
            writer.Write((int) type);
            int len = clips?.Length ?? 0;
            int len2 = childs?.Length ?? 0;
            int len3 = marks?.Length ?? 0;
            writer.Write(len);
            writer.Write(len2);
            writer.Write(len3);
            for (int j = 0; j < len; j++)
            {
                clips[j].Write(writer);
            }
            for (int j = 0; j < len2; j++)
            {
                childs[j].Write(writer);
            }
            for (int i = 0; i < len3; i++)
            {
                marks[i].Write(writer);
            }
        }

        public virtual void Read(BinaryReader reader)
        {
            int len = reader.ReadInt32();
            if (len > 0) clips = new ClipData[len];
            int len2 = reader.ReadInt32();
            if (len2 > 0) childs = new TrackData[len2];
            int len3 = reader.ReadInt32();
            if (len3 > 0) marks = new MarkData[len3];

            for (int j = 0; j < len; j++)
            {
                clips[j] = XTimelineFactory.CreateClipData(reader);
            }
            for (int j = 0; j < len2; j++)
            {
                childs[j] = new TrackData((TrackType) reader.ReadInt32());
                childs[j].Read(reader);
            }
            for (int i = 0; i < len3; i++)
            {
                marks[i] = XTimelineFactory.CreateMarkData(reader);
            }
        }
    }

    public class BindTrackData : TrackData
    {
        public string prefab;

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(prefab);
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            prefab = reader.ReadString();
        }

        public BindTrackData(TrackType t) : base(t)
        {
        }
    }

    public class TimelineConfig
    {
        public TrackData[] tracks;

        public void Write(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                int cnt = tracks.Length;
                writer.Write(cnt);
                for (int i = 0; i < cnt; i++)
                {
                    tracks[i].Write(writer);
                }
                writer.Flush();

#if UNITY_EDITOR
                AssetDatabase.ImportAsset(path);
#endif
            }
        }

        public void Read(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    int cnt = reader.ReadInt32();
                    tracks = new TrackData[cnt];
                    for (int i = 0; i < cnt; i++)
                    {
                        var type = (TrackType) reader.ReadInt32();
                        tracks[i] = new TrackData(type);
                        tracks[i].Read(reader);
                    }
                }
            }
        }
        
    }
}
