using System;

namespace UnityEngine.Timeline
{

    [AttributeUsage(AttributeTargets.Class)]
    public class TrackAttribute : Attribute
    {

        public string desc;

        public bool recordable;


        public TrackAttribute(string desc, bool record)
        {
            this.desc = desc;
            this.recordable = record;
        }

    }
}