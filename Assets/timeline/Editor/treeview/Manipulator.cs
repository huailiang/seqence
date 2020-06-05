using UnityEngine;

namespace UnityEditor.Timeline
{
    public abstract class Manipulator
    {
        int m_Id;

        protected virtual bool MouseDown(Event evt) { return false; }
        protected virtual bool MouseDrag(Event evt) { return false; }
        protected virtual bool MouseWheel(Event evt) { return false; }
        protected virtual bool MouseUp(Event evt) { return false; }
        protected virtual bool DoubleClick(Event evt) { return false; }
        protected virtual bool KeyDown(Event evt) { return false; }
        protected virtual bool KeyUp(Event evt) { return false; }
        protected virtual bool ContextClick(Event evt) { return false; }
        protected virtual bool ValidateCommand(Event evt) { return false; }
        protected virtual bool ExecuteCommand(Event evt) { return false; }

        public virtual void Overlay(Event evt, TimelineWindow state) {}

        public bool HandleEvent(TimelineWindow state)
        {
            if (m_Id == 0)
                m_Id =  GUIUtility.GetPermanentControlID();

            bool isHandled = false;
            var evt = Event.current;

            switch (evt.GetTypeForControl(m_Id))
            {
                case EventType.ScrollWheel:
                    isHandled = MouseWheel(evt);
                    break;

                case EventType.MouseUp:
                {
                    if (GUIUtility.hotControl == m_Id)
                    {
                        isHandled = MouseUp(evt);

                        GUIUtility.hotControl = 0;
                        evt.Use();
                    }
                }
                break;

                case EventType.MouseDown:
                {
                    isHandled = evt.clickCount < 2 ? MouseDown(evt) : DoubleClick(evt);

                    if (isHandled)
                        GUIUtility.hotControl = m_Id;
                }
                break;

                case EventType.MouseDrag:
                {
                    if (GUIUtility.hotControl == m_Id)
                        isHandled = MouseDrag(evt);
                }
                break;

                case EventType.KeyDown:
                    isHandled = KeyDown(evt);
                    break;

                case EventType.KeyUp:
                    isHandled = KeyUp(evt);
                    break;

                case EventType.ContextClick:
                    isHandled = ContextClick(evt);
                    break;

                case EventType.ValidateCommand:
                    isHandled = ValidateCommand(evt);
                    break;

                case EventType.ExecuteCommand:
                    isHandled = ExecuteCommand(evt);
                    break;
            }

            if (isHandled)
                evt.Use();

            return isHandled;
        }
    }
}
