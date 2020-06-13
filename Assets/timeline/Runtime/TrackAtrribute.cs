using System;
using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TrackDescriptorAttribute : Attribute
    {
        public bool isOnlySub;
        
        public TrackDescriptorAttribute( bool onlySub)
        {
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


    [AttributeUsage(AttributeTargets.Class)]
    public class UseParentAttribute : Attribute
    {
        public Type parent;

        public UseParentAttribute(Type t)
        {
            parent = t;
        }
    }


    [AttributeUsage(AttributeTargets.Class)]
    public class MarkUsageAttribute : Attribute
    {
        public TrackType type;

        public MarkUsageAttribute(TrackType t)
        {
            this.type = t;
        }
    }
}
