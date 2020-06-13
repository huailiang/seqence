using System;

namespace UnityEditor.Timeline
{
    
    [AttributeUsage(AttributeTargets.Class)]
    public class TrackEditorAttribute: Attribute
    {
        public Type type;

        public TrackEditorAttribute(Type t)
        {
            type = t;
        }
        
    }
}
