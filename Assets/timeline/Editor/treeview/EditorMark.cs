using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public class EditorMark
    {
        protected XMarker baseMarker;

        public void Init(XMarker marker)
        {
            this.baseMarker = marker;
            OnInit();
        }

        protected virtual void OnInit()
        {
        }

        public void Inspector()
        {
            EditorGUILayout.LabelField(baseMarker.type.ToString());
            baseMarker.time = EditorGUILayout.FloatField("time", baseMarker.time);
            baseMarker.reverse = EditorGUILayout.Toggle("reverse", baseMarker.reverse);
            OnInspector();
        }

        protected virtual void OnInspector()
        {
        }
    }

    [TimelineEditor(typeof(XJumpMarker))]
    public class EditorJumpMark : EditorMark
    {
        private XJumpMarker marker;

        protected override void OnInit()
        {
            this.marker = (XJumpMarker) baseMarker;
        }

        protected override void OnInspector()
        {
            marker.jump = EditorGUILayout.FloatField("jump:", marker.jump);
        }
    }

    [TimelineEditor(typeof(XActiveMark))]
    public class EditorActiveMark : EditorMark
    {
        private XActiveMark marker;

        protected override void OnInit()
        {
            this.marker = (XActiveMark) baseMarker;
        }

        protected override void OnInspector()
        {
            marker.active = EditorGUILayout.Toggle("active", marker.active);
        }
    }

    [TimelineEditor(typeof(XSlowMarker))]
    public class EditorSlowMark : EditorMark
    {
        private XSlowMarker marker;

        protected override void OnInit()
        {
            this.marker = (XSlowMarker) baseMarker;
        }

        protected override void OnInspector()
        {
            marker.slow = EditorGUILayout.FloatField("slowRate", marker.slow);
        }
    }
}
