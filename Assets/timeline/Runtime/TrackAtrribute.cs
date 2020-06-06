using System;

namespace UnityEngine.Timeline
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TrackDescriptorAttribute : Attribute
    {
        public string desc;

        public bool recordable;


        public TrackDescriptorAttribute(string desc, bool record)
        {
            this.desc = desc;
            this.recordable = record;
        }
    }

    public class TrackRequreType : Attribute
    {
        public Type type;

        public TrackRequreType(Type t)
        {
            type = t;
        }
    }
}
