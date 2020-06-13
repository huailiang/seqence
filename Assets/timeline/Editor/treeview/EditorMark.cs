using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public class EditorMark
    {
        private XMarker baseMarker;

        public EditorMark(XMarker marker)
        {
            baseMarker = marker;
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

        public EditorJumpMark(XJumpMarker marker) : base(marker)
        {
            this.marker = marker;
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

        public EditorActiveMark(XActiveMark marker) : base(marker)
        {
            this.marker = marker;
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

        public EditorSlowMark(XSlowMarker marker) : base(marker)
        {
            this.marker = marker;
        }

        protected override void OnInspector()
        {
            marker.slow = EditorGUILayout.FloatField("slowRate", marker.slow);
        }
    }
}
