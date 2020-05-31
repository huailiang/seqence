using System;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Timeline
{
    class TimeAreaItem : Control
    {
        public Color headColor { get; set; }
        public Color lineColor { get; set; }
        public bool drawLine { get; set; }
        public bool drawHead { get; set; }
        public bool canMoveHead { get; set; }
        public string tooltip { get; set; }
        public Vector2 boundOffset { get; set; }

        readonly GUIContent m_HeaderContent = new GUIContent();
        readonly GUIStyle m_Style;
        // readonly Tooltip m_Tooltip;
    }
}
