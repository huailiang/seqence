using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public struct TrackMenuAction
    {
        public string desc;
        public bool on;
        public GenericMenu.MenuFunction2 fun;
        public object arg;
    }

    public class EditorTrack
    {
        public XTrack track;
        public Rect rect;
        public Rect head;
        public bool select;
        private GenericMenu pm;

        public uint ID
        {
            get { return track.ID; }
        }

        protected virtual Color trackColor
        {
            get { return Color.red; }
        }

        protected virtual List<TrackMenuAction> actions { get; }

        protected bool triger
        {
            get
            {
                var pos = Event.current.mousePosition;
                return head.Contains(pos) || rect.Contains(pos);
            }
        }

        public void OnGUI()
        {
            var backgroundColor = select
                ? TimelineStyles.colorDuration
                : TimelineStyles.markerHeaderDrawerBackgroundColor;

            var headColor = backgroundColor;
            EditorGUI.DrawRect(head, headColor);
            Rect tmp = head;
            tmp.width = 4;
            EditorGUI.DrawRect(tmp, trackColor);

            EditorGUI.DrawRect(rect, backgroundColor);
            tmp = rect;
            tmp.height = 2;
            tmp.y = rect.y + rect.height - 2;
            EditorGUI.DrawRect(tmp, trackColor * 0.9f);

            GUIHeader();
            GUIContent();
            if (!track.locked)
            {
                ProcessEvent();
            }
        }

        protected void ProcessEvent()
        {
            var e = Event.current;
            if (e.type == EventType.ContextClick)
            {
                if (triger)
                {
                    pm = new GenericMenu();
                    if (TimelineWindow.inst.tree.AnySelect())
                        pm.AddItem(EditorGUIUtility.TrTextContent("UnSelect All"), false, UnSelectAll);
                    else
                        pm.AddDisabledItem(EditorGUIUtility.TrTextContent("UnSelect All"));
                    pm.AddSeparator("");
                    pm.AddItem(EditorGUIUtility.TrTextContent("Add Clip \t"), false, AddClip, e.mousePosition);
                    pm.AddItem(EditorGUIUtility.TrTextContent("Delete \t"), false, DeleteClip);
                    if (track.mute)
                    {
                        pm.AddItem(EditorGUIUtility.TrTextContent("UnMute Track \t"), false, UnmuteClip);
                    }
                    else
                    {
                        pm.AddItem(EditorGUIUtility.TrTextContent("Mute Track \t"), false, MuteClip);
                    }
                    pm.AddSeparator("");
                    if (actions != null)
                    {
                        for (int i = 0; i < actions.Count; i++)
                        {
                            var at = actions[i];
                            pm.AddItem(EditorGUIUtility.TrTextContent(at.desc), at.@on, at.fun, at.arg);
                        }
                    }
                    pm.ShowAsContext();
                }
            }
            else if (e.type == EventType.MouseUp)
            {
                if (triger)
                {
                    @select = !@select;
                    TimelineWindow.inst.Repaint();
                }
            }
        }

        public void YOffset(float y)
        {
            head.y += y;
            rect.y += y;
        }

        public void SetHeight(float height)
        {
            head.height = height;
            rect.height = height;
        }

        protected void GUIHeader()
        {
            var tmp = head;
            tmp.y += head.height / 4;
            GUILayout.BeginArea(tmp);
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Label(track.ToString());
            OnGUIHeader();
            if (GUILayout.Button("mute", TimelineStyles.mute))
            {
                track.SetFlag(TrackMode.Mute, !track.mute);
            }
            if (GUILayout.Button("lock", TimelineStyles.locked))
            {
                track.SetFlag(TrackMode.Lock, !track.locked);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        protected virtual void OnGUIHeader()
        {
        }

        protected void GUIContent()
        {
            var clips = track.clips;
            if (clips != null)
            {
                for (int i = 0; i < clips.Length; i++)
                {
                    new EditorClip(this, clips[i]).OnGUI();
                }
            }
            if (track.locked)
            {
                GUI.Box(rect, "", TimelineStyles.lockedBG);
            }
            GUILayout.BeginArea(rect);
            OnGUIContent();
            GUILayout.EndArea();
        }

        protected virtual void OnGUIContent()
        {
        }

        private void UnSelectAll()
        {
            TimelineWindow.inst.tree?.ResetSelect();
        }

        protected virtual void AddClip(object mpos)
        {
            TimelineWindow.inst.Repaint();
        }

        private void DeleteClip()
        {
            Debug.Log("delete Click");
            TimelineWindow.inst.Repaint();
        }

        private void MuteClip()
        {
            track.SetFlag(TrackMode.Mute, true);
            TimelineWindow.inst.Repaint();
        }

        private void UnmuteClip()
        {
            track.SetFlag(TrackMode.Mute, false);
            TimelineWindow.inst.Repaint();
        }
    }
}
