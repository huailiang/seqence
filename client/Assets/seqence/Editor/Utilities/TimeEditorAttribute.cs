using System;

namespace UnityEditor.Seqence
{
    
    [AttributeUsage(AttributeTargets.Class)]
    public class TimelineEditorAttribute: Attribute
    {
        public Type type;

        public TimelineEditorAttribute(Type t)
        {
            type = t;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TimelineClipEditorAttribute : Attribute
    {
        public Type type;

        public TimelineClipEditorAttribute(Type t)
        {
            type = t;
        }
    }
}
