using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public class EditorAnimTrack : EditorTrack
    {
        protected override Color trackColor
        {
            get { return Color.yellow; }
        }

        protected override void OnGUIHeader()
        {
            XBindTrack btrack = track as XBindTrack;
            GUILayout.Space(4);
            if (GUILayout.Button("rec", GUILayout.MaxWidth(16)))
            {
                Debug.Log("recod mode");
            }
            GUILayout.EndArea();
            if (btrack && btrack.bindObj)
            {
#pragma warning disable 618
                EditorGUILayout.ObjectField(btrack.bindObj, typeof(Animator), GUILayout.MaxWidth(10));
#pragma warning restore 618
            }
        }

        protected override void OnGUIContent()
        {
        }
    }
}
