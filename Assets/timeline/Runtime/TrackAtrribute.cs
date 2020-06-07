using System;

namespace UnityEngine.Timeline
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TrackDescriptorAttribute : Attribute
    {
        public string desc;

        public bool isOnlySub;


        public TrackDescriptorAttribute(string desc, bool onlySub)
        {
            this.desc = desc;
            isOnlySub = onlySub;
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
