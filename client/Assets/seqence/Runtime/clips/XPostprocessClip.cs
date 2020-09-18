using UnityEngine.Seqence.Data;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.Seqence
{
    public class XPostprocessClip : XClip<XPostprocessTrack, XPostprocessClip>, ISharedObject<XPostprocessClip>
    {
        private PostprocessData Data;

        public override string Display
        {
            get
            {
                if (data != null)
                {
                    Data = data as PostprocessData;
                    return Data.ToString();
                }
                return "post process";
            }
        }

        public override void OnDestroy()
        {
            SharedPool<XPostprocessClip>.Return(this);
            base.OnDestroy();
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            Data = data as PostprocessData;
        }

#if UNITY_EDITOR


        public void OnInspector()
        {
            Data = data as PostprocessData;
            if (Data != null)
            {
                Data.mode = (PostEnum)EditorGUILayout.EnumPopup("Effect", Data.mode);
            }
        }

#endif
    }

}