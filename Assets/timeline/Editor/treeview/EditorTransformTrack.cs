using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [TimelineEditor(typeof(XTransformTrack))]
    public class EditorTransformTrack : EditorTrack
    {
        protected override Color trackColor
        {
            get { return new Color(0.8f, 0.7f, 0.2f); }
        }

        protected override string trackHeader
        {
            get { return "位移" + ID; }
        }

        protected override void OnAddClip(float time)
        {
            throw new Exception("transform no clips");
        }
    }
}
