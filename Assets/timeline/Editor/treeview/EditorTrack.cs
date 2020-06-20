using System;
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

    public class EditorTrack : EditorObject, ITimelineInspector
    {
        public XTrack track;
        public Rect rect, head;
        public EditorClip[] eClips;
        public bool select, allowClip, showChild;
        private GenericMenu pm;
        private GUIContent _addclip, _unselect, _select, _delete;

        public uint ID
        {
            get { return track.ID; }
        }

        protected virtual Color trackColor
        {
            get { return Color.red; }
        }

        protected virtual string trackHeader
        {
            get { return track + " " + ID; }
        }

        protected bool locked
        {
            get { return track.locked; }
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

        public override void OnInit(XTimelineObject t)
        {
            @select = false;
            showChild = true;
            track = (XTrack) t;
            var flag = (TrackFlagAttribute) Attribute.GetCustomAttribute(t.GetType(), typeof(TrackFlagAttribute));
            allowClip = flag.allowClip;
            _addclip = EditorGUIUtility.TrTextContent("Add Clip \t #a");
            _unselect = EditorGUIUtility.TrTextContent("UnSelect All  \t #u");
            _select = EditorGUIUtility.TrTextContent("Select All Tracks \t %#s");
            _delete = EditorGUIUtility.TrTextContent("Delete Clip\t #d");
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
            ProcessEvent();
        }

        protected void ProcessEvent()
        {
            var e = Event.current;
            if (e.type == EventType.ContextClick)
            {
                if (triger)
                {
                    TrackContexMenu(e);
                }
            }
        }

        private void TrackContexMenu(Event e)
        {
            pm = new GenericMenu();
            if (TimelineWindow.inst.tree.AnySelect())
            {
                pm.AddItem(_unselect, false, UnSelectAll, false);
                pm.AddDisabledItem(_select);
            }
            else
            {
                pm.AddItem(_select, false, UnSelectAll, true);
                pm.AddDisabledItem(_unselect);
            }

            pm.AddSeparator("");
            if (allowClip && !locked)
            {
                pm.AddItem(_addclip, false, AddClip, e.mousePosition);
                pm.AddItem(_delete, false, DeleteClip, e.mousePosition);
            }
            else
            {
                pm.AddDisabledItem(_addclip, false);
                pm.AddDisabledItem(_delete, false);
            }

            pm.AddItem(EditorGUIUtility.TrTextContent("Delete Track\t #t"), false, DeleteTrack);
            if (track.mute)
            {
                pm.AddItem(EditorGUIUtility.TrTextContent("UnMute Track \t "), false, UnmuteClip);
            }
            else
            {
                pm.AddItem(EditorGUIUtility.TrTextContent("Mute Track \t"), false, MuteClip);
            }
            if (locked)
            {
                pm.AddItem(EditorGUIUtility.TrTextContent("UnLock Track \t #l"), false,
                    () => track.SetFlag(TrackMode.Lock, false));
            }
            else
            {
                pm.AddItem(EditorGUIUtility.TrTextContent("Lock Track \t #l"), false,
                    () => track.SetFlag(TrackMode.Lock, true));
            }
            if (@select)
            {
                pm.AddItem(EditorGUIUtility.TrTextContent("UnSelect Track \t #s"), false, SelectTrack, false);
            }
            else
            {
                pm.AddItem(EditorGUIUtility.TrTextContent("Select Track \t #s"), false, SelectTrack, true);
            }
            pm.AddSeparator("");
            if (actions != null)
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    var at = actions[i];
                    if (!locked) pm.AddItem(EditorGUIUtility.TrTextContent(at.desc), at.@on, at.fun, at.arg);
                }
            }
            pm.AddSeparator("");
            var marks = TypeUtilities.GetBelongMarks(track.AssetType);
            for (int i = 0; i < marks.Count; i++)
            {
                var mark = marks[i];
                string str = mark.ToString();
                int idx = str.LastIndexOf('.');
                str = str.Substring(idx + 1);
                var ct = EditorGUIUtility.TrTextContent("Add " + str);
                MarkAction action = new MarkAction() {type = mark, posX = e.mousePosition.x};
                if (!locked) pm.AddItem(ct, false, AddMark, action);
            }
            pm.ShowAsContext();
            e.Use();
        }

        private void SelectTrack(object arg)
        {
            bool sele = (bool) arg;
            this.@select = sele;
            TimelineWindow.inst.Repaint();
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
            GUILayout.Label(trackHeader);
            OnGUIHeader();
            if (track.mute)
                if (GUILayout.Button(TimelineStyles.empty, TimelineStyles.mute))
                    track.SetFlag(TrackMode.Mute, false);
            if (track.locked)
                if (GUILayout.Button(TimelineStyles.empty, TimelineStyles.locked))
                    track.SetFlag(TrackMode.Lock, false);
            var tree = TimelineWindow.inst.tree;
            int idx = tree.IndexOfTrack(track);
            if (track.hasChilds)
            {
                if (GUILayout.Button(TimelineStyles.sequenceSelectorIcon, TimelineStyles.bottomShadow))
                {
                    if (showChild)
                        tree.RmChildTrack(this);
                    else
                        tree.AddChildTracks(track);
                    showChild = !showChild;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        protected virtual void OnGUIHeader()
        {
        }


        protected void GUIContent()
        {
            var marks = track.marks;
            if (marks != null)
            {
                SetupEMarks();
                foreach (var mark in emarks)
                {
                    mark.ProcessEvent();
                }
            }
            var clips = track.clips;
            if (clips != null)
            {
                int len = clips.Length;
                if (eClips == null || eClips.Length != len)
                {
                    eClips = new EditorClip[len];
                    for (int i = 0; i < len; i++)
                    {
                        eClips[i] = new EditorClip(this, clips[i]);
                    }
                }
                for (int i = 0; i < clips.Length; i++)
                {
                    eClips[i].OnGUI();
                }
            }
            if (marks != null)
            {
                for (int i = 0; i < marks.Length; i++)
                {
                    emarks[i].OnGUI(rect);
                }
            }
            if (track.locked)
            {
                GUI.Box(rect, "", TimelineStyles.lockedBG);
            }
            OnGUIContent();
        }

        void DrawMarkItem(XMarker mark)
        {
            float x = TimelineWindow.inst.TimeToPixel(mark.time);
            Rect tmp = rect;
            tmp.x = x;
            tmp.y = rect.y + rect.height / 4;
            tmp.width = 20;
            GUIContent cont = TimelineWindow.inst.state.config.GetIcon(mark.type);
            GUI.Box(tmp, cont, GUIStyle.none);
        }

        protected virtual void OnGUIContent()
        {
        }

        public void UnSelectAll(object arg)
        {
            bool selet = (bool) arg;
            TimelineWindow.inst.tree?.ResetSelect(selet);
        }

        private void AddMark(object m)
        {
            MarkAction t = (MarkAction) m;
            float time = TimelineWindow.inst.PiexlToTime(t.posX);
            EditorFactory.MakeMarker(t.type, time, track);
        }


        private void AddClip(object mpos)
        {
            Vector2 v2 = (Vector2) mpos;
            float start = TimelineWindow.inst.PiexlToTime(v2.x);
            OnAddClip(start);
            TimelineWindow.inst.Repaint();
        }

        protected virtual void OnAddClip(float time)
        {
        }

        private void DeleteClip(object mpos)
        {
            Vector2 pos = (Vector2) mpos;
            float time = TimelineWindow.inst.PiexlToTime(pos.x);
            bool find = false;
            if (track.clips != null)
            {
                for (int i = 0; i < track.clips.Length; i++)
                {
                    var clip = track.clips[i];
                    if (time >= clip.start && time <= clip.end)
                    {
                        track.RmClip(clip);
                        find = true;
                        TimelineWindow.inst.Repaint();
                        break;
                    }
                }
            }
            if (!find)
            {
                EditorUtility.DisplayDialog("tip", "not select clip", "ok");
            }
        }

        private void DeleteTrack()
        {
            TimelineWindow.inst.tree.RmTrack(this);
            track.Remove(TimelineWindow.inst.timeline);
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


        private bool trackF;
        private EditorMark[] emarks;

        private void SetupEMarks()
        {
            int len = track.marks.Length;
            if (emarks == null || emarks.Length != len)
            {
                emarks = new EditorMark[len];
                for (int j = 0; j < len; j++)
                {
                    emarks[j] = (EditorMark) TypeUtilities.InitEObject(track.marks[j]);
                }
            }
        }

        public void OnInspector()
        {
            using (GUIColorOverride color = new GUIColorOverride(Color.green))
            {
                trackF = EditorGUILayout.Foldout(trackF, trackHeader);
            }
            if (trackF)
            {
                OnInspectorTrack();
                int i = 0;
                if (track.clips != null)
                {
                    foreach (var clip in track.clips)
                    {
                        EditorGUILayout.LabelField(" clip" + (++i) + ": " + clip.Display, TimelineStyles.titleStyle);
                        OnInspectorClip(clip);
                    }
                }
                if (track.marks != null)
                {
                    SetupEMarks();
                    foreach (var mark in emarks)
                    {
                        mark.Inspector();
                    }
                }
            }
        }

        protected virtual void OnInspectorTrack()
        {
        }

        protected virtual void OnInspectorClip(IClip clip)
        {
            clip.start = EditorGUILayout.FloatField(" start", clip.start);
            float d = EditorGUILayout.FloatField(" duration", clip.duration);
            if (d > 0) clip.duration = d;
        }
    }
}
