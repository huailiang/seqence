using System;

namespace UnityEditor.Seqence
{
    
    [AttributeUsage(AttributeTargets.Class)]
    public class SeqenceEditorAttribute: Attribute
    {
        public Type type;

        public SeqenceEditorAttribute(Type t)
        {
            type = t;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class SeqenceClipEditorAttribute : Attribute
    {
        public Type type;

        public SeqenceClipEditorAttribute(Type t)
        {
            type = t;
        }
    }
}
