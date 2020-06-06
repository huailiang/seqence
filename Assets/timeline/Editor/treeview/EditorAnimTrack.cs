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
            if (GUILayout.Button("rec", GUILayout.MaxWidth(icoWdt)))
            {
                Debug.Log("start recod mode");
                track.SetFlag(TrackMode.Record, !track.record);
            }
            if (GUILayout.Button("clip", GUILayout.MaxWidth(icoWdt)))
            {
                Debug.Log("show record clip");
            }
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
