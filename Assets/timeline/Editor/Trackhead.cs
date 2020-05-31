using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
    class Scrub : Manipulator
    {
        readonly Func<Event, TimelineWindow, bool> m_OnMouseDown;
        readonly Action<double> m_OnMouseDrag;
        readonly Action m_OnMouseUp;

        bool m_IsCaptured;

        public Scrub(Func<Event, TimelineWindow, bool> onMouseDown, Action<double> onMouseDrag, Action onMouseUp)
        {
            m_OnMouseDown = onMouseDown;
            m_OnMouseDrag = onMouseDrag;
            m_OnMouseUp = onMouseUp;
        }

        protected override bool MouseDown(Event evt, TimelineWindow state)
        {
            if (evt.button != 0) return false;

            if (!m_OnMouseDown(evt, state)) return false;

            // state.AddCaptured(this);
            m_IsCaptured = true;

            return true;
        }

        protected override bool MouseUp(Event evt, TimelineWindow state)
        {
            if (!m_IsCaptured) return false;

            m_IsCaptured = false;
            // state.RemoveCaptured(this);

            m_OnMouseUp();

            return true;
        }

        protected override bool MouseDrag(Event evt, TimelineWindow state)
        {
            if (!m_IsCaptured) return false;

            m_OnMouseDrag(state.GetSnappedTimeAtMousePosition(evt.mousePosition));
            return true;
        }
    }

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
        readonly Tooltip m_Tooltip;

        Rect m_BoundingRect;

        float widgetHeight
        {
            get
            {
                var height = m_Style.fixedHeight;
                if (height < 1.0f) return m_Style.normal.background.height;
                return height;
            }
        }

        float widgetWidth
        {
            get
            {
                var width = m_Style.fixedWidth;
                if (width < 1.0f) return m_Style.normal.background.width;
                return width;
            }
        }

        public Rect bounds
        {
            get
            {
                Rect r = m_BoundingRect;
                r.y = TimelineWindow.inst.timeAreaRect.yMax - widgetHeight;
                r.position += boundOffset;
                return r;
            }
        }

        public GUIStyle style
        {
            get { return m_Style; }
        }

        public bool showTooltip { get; set; }

        // is this the first frame the drag callback is being invoked
        public bool firstDrag { get; private set; }

        public TimeAreaItem(GUIStyle style, Action<double> onDrag)
        {
            m_Style = style;
            headColor = Color.white;
            var scrub = new Scrub((evt, state) =>
            {
                firstDrag = true;
                return state.timeAreaRect.Contains(evt.mousePosition) && bounds.Contains((Vector2) evt.mousePosition);
            }, (d) =>
            {
                onDrag?.Invoke(d);
                firstDrag = false;
            }, () =>
            {
                showTooltip = false;
                firstDrag = false;
            });
            AddManipulator(scrub);
            lineColor = m_Style.normal.textColor;
            drawLine = true;
            drawHead = true;
            canMoveHead = false;
            tooltip = string.Empty;
            boundOffset = Vector2.zero;
            m_Tooltip = new Tooltip(TimelineStyles.displayBackground, TimelineStyles.tinyFont);
        }

        public void Draw(Rect rect, TimelineWindow win, float time)
        {
            var clipRect = new Rect(0.0f, 0.0f, win.position.width, win.position.height);
            clipRect.xMin += win.sequencerHeaderWidth;

            using (new GUIViewportScope(clipRect))
            {
                Vector2 windowCoordinate = rect.min;
                windowCoordinate.y += 4.0f;
                windowCoordinate.x = win.TimeToPixel(time);
                m_BoundingRect = new Rect((windowCoordinate.x - widgetWidth / 2.0f), windowCoordinate.y, widgetWidth,
                    widgetHeight);

                // Do not paint if the time cursor goes outside the timeline bounds...
                if (Event.current.type == EventType.Repaint)
                {
                    if (m_BoundingRect.xMax < win.timeAreaRect.xMin) return;
                    if (m_BoundingRect.xMin > win.timeAreaRect.xMax) return;
                }

                var top = new Vector3(windowCoordinate.x, rect.y - TimelineStyles.kDurationGuiThickness);
                var bottom = new Vector3(windowCoordinate.x, rect.yMax);
                if (drawLine)
                {
                    Rect lineRect = Rect.MinMaxRect(top.x - 0.5f, top.y, bottom.x + 0.5f, bottom.y);
                    EditorGUI.DrawRect(lineRect, lineColor);
                }
                if (drawHead)
                {
                    Color c = GUI.color;
                    GUI.color = headColor;
                    GUI.Box(bounds, m_HeaderContent, m_Style);
                    GUI.color = c;
                    if (canMoveHead) EditorGUIUtility.AddCursorRect(bounds, MouseCursor.MoveArrow);
                }

                if (showTooltip)
                {
                    m_Tooltip.text = time.ToString("f1");
                    Vector2 position = bounds.position;
                    position.y = win.timeAreaRect.y;
                    position.y -= m_Tooltip.bounds.height;
                    position.x -= Mathf.Abs(m_Tooltip.bounds.width - bounds.width) / 2.0f;
                    Rect tooltipBounds = bounds;
                    tooltipBounds.position = position;
                    m_Tooltip.bounds = tooltipBounds;
                    m_Tooltip.Draw();
                }
            }
        }
    }
}
