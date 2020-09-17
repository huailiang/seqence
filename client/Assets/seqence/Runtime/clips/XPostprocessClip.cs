using UnityEngine.Timeline.Data;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.Timeline
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

        private PostEnum add_p = 0;

        public void OnInspector()
        {
            int cnt = PostprocessData.PostCnt;
            Data = data as PostprocessData;
            EditorGUILayout.BeginVertical(EditorStyles.label);
            for (int i = 0; i < cnt; i++)
            {
                if (Data != null)
                {
                    if (((int)Data.mode & (1 << i)) > 0)
                    {
                        EditorGUILayout.BeginHorizontal();
                        var em = (PostEnum)(1 << i);
                        EditorGUILayout.EnumPopup("Effect", em);
                        if (GUILayout.Button("X", GUILayout.MaxWidth(21)))
                        {
                            Data.mode &= ~em;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            add_p = (PostEnum)EditorGUILayout.EnumPopup("Add-new", add_p);
            if (EditorGUI.EndChangeCheck())
            {
                Data.mode |= add_p;
                add_p = 0;
            }
        }
#endif
    }

}