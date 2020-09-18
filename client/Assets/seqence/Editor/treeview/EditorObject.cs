using UnityEngine.Seqence;

namespace UnityEditor.Seqence
{
    public abstract class EditorObject
    {

        public abstract void OnInit(XSeqenceObject obj);

        public static implicit operator bool(EditorObject obj)
        {
            return obj != null;
        }
    }
}
