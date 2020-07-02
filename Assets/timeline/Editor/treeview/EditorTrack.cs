using System;
using System.Collections.Generic;
using System.Linq;
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
        private Vector2 scroll;
        protected Color addtiveColor;
        public object trackArg = null;
        protected Event e;

        public uint ID
        {
            get { return track.ID; }
        }

        protected virtual bool ignoreDraw
        {
            get { return false; }
        }

        protected virtual Color trackColor
        {
            get { return Color.red; }
        }

        protected virtual string trackHeader
        {
            get { return track.ToString(); }
        }

        protected virtual bool warn
        {
            get { return track.clips == null; }
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
                var e = Event.current;
                if (e != null)
                {
                    var pos = e.mousePosition;
                    return RenderHead.Contains(pos) || RenderRect.Contains(pos);
                }
                return false;
            }
        }

        private float Offset
        {
            get
            {
                if (TimelineWindow.inst.state.showMarkerHeader)
                {
                    return WindowConstants.trackRowYPosition;
                }
                return WindowConstants.markerRowYPosition;
            }
        }

        protected Rect RenderHead
        {
            get
            {
                Rect r = head;
                r.y -= scroll.y + Offset;
                return r;
            }
        }

        public Rect RenderRect
        {
            get
            {
                Rect r = rect;
                r.y -= scroll.y + Offset;
                return r;
            }
        }

        private bool dragingClip
        {
            get
            {
                if (eClips != null)
                {
                    foreach (var clip in eClips)
                    {
                        if (clip.dragMode != DragMode.None) return true;
                    }
                }
                return false;
            }
        }

        public void SetRect(Rect h, Rect c)
        {
            this.head = h;
            this.rect = c;
        }


        public override void OnInit(XTimelineObject t)
        {
            @select = false;
            showChild = true;
            addtiveColor = Color.white;
            track = (XTrack)t;
            var flag = (TrackFlagAttribute)Attribute.GetCustomAttribute(t.GetType(), typeof(TrackFlagAttribute));
            allowClip = flag.allowClip;
            _addclip = EditorGUIUtility.TrTextContent("Add Clip \t #a");
            _unselect = EditorGUIUtility.TrTextContent("UnSelect All  \t #u");
            _select = EditorGUIUtility.TrTextContent("Select All Tracks \t %#s");
            _delete = EditorGUIUtility.TrTextContent("Delete Clip\t #d");
        }

        public void OnGUI(Vector2 scroll)
        {
            e = Event.current;
            this.scroll = scroll;
            var backgroundColor = select ? TimelineStyles.colorDuration : TimelineStyles.BackgroundColor;
            backgroundColor *= addtiveColor;

            var headColor = backgroundColor;
            EditorGUI.DrawRect(RenderHead, headColor);
            Rect tmp = RenderHead;
            tmp.width = 4;
            if (!ignoreDraw) EditorGUI.DrawRect(tmp, trackColor);

            EditorGUI.DrawRect(RenderRect, backgroundColor);
            tmp = RenderRect;
            tmp.y += tmp.height - 2;
            tmp.height = 1;
            if (!ignoreDraw) EditorGUI.DrawRect(tmp, trackColor);

            GUIContent();
            GUIHeader();
            if (!ignoreDraw) ProcessEvent();
        }

        protected void ProcessEvent()
        {
            if (triger)
            {
                switch (e.type)
                {
                    case EventType.ContextClick:
                        TrackContexMenu(e);
                        break;
                    case EventType.MouseDown:
                        if (e.button == 0 && !dragingClip)
                        {
                            if (e.shift)
                            {
                                TimelineWindow.inst.tree?.ShiftSelects(this);
                            }
                            else
                            {
                                UnSelectAll(false);
                                SelectTrack(true);
                            }
                        }
                        break;
                    case EventType.KeyUp:
                        if (e.keyCode == KeyCode.Delete)
                        {
                            if (HitClip(e)) DeleteClip(e.mousePosition);
                            else DeleteTrack(this);
                            e.Use();
                        }
                        break;
                    case EventType.DragExited:
                        e.Use();
                        break;
                    case EventType.DragPerform:
                    case EventType.DragUpdated:
                        if (DragAndDrop.objectReferences.Count() > 0 &&
                            RenderRect.Contains(e.mousePosition))
                            OnDragDrop(DragAndDrop.objectReferences);
                        break;
                }
            }
        }


        protected virtual void OnSelect() { }

        protected virtual void OnGUIHeader() { }

        protected virtual void OnGUIContent() { }

        protected virtual void OnAddClip(float time) { }

        protected virtual void OnInspectorTrack() { }

        protected virtual void OnDragDrop(UnityEngine.Object[] objs) { }


        private bool HitClip(Event e)
        {
            if (eClips != null)
            {
                foreach (var c in eClips)
                {
                    if (c.rect.Contains(e.mousePosition))
                    {
                        return true;
                    }
                }
            }
            return false;
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
                if (HitClip(e))
                    pm.AddItem(_delete, false, DeleteClip, e.mousePosition);
                else
                    pm.AddDisabledItem(_delete, false);
            }
            else
            {
                pm.AddDisabledItem(_addclip, false);
                pm.AddDisabledItem(_delete, false);
            }
            pm.AddItem(EditorGUIUtility.TrTextContent("Delete Track\t #t"), false, DeleteTrack);
            if (track.mute)
            {
                pm.AddItem(EditorGUIUtility.TrTextContent("UnMute Track \t "), false, () => SetTrackFlag(TrackMode.Mute, false));
            }
            else
            {
                pm.AddItem(EditorGUIUtility.TrTextContent("Mute Track \t"), false, () => SetTrackFlag(TrackMode.Mute, true));
            }
            if (locked)
            {
                pm.AddItem(EditorGUIUtility.TrTextContent("UnLock Track \t #l"), false, () => SetTrackFlag(TrackMode.Lock, false));
            }
            else
            {
                pm.AddItem(EditorGUIUtility.TrTextContent("Lock Track \t #l"), false, () => SetTrackFlag(TrackMode.Lock, true));
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
                MarkAction action = new MarkAction() { type = mark, posX = e.mousePosition.x };
                if (!locked) pm.AddItem(ct, false, AddMark, action);
            }
            pm.ShowAsContext();
            e.Use();
        }

        private void SelectTrack(object arg)
        {
            bool sele = (bool)arg;
            this.@select = sele;
            OnSelect();
            TimelineWindow.inst.tree?.SetSelect(this);
            TimelineWindow.inst.Repaint();
        }

        public void YOffset(float y)
        {
            head.y += y;
            rect.y += y;
        }

        protected void GUIHeader()
        {
            var tmp = RenderHead;
            tmp.y += tmp.height / 4;
            GUILayout.BeginArea(tmp);
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Label(trackHeader);
            OnGUIHeader();
            if (track.mute)
            {
                GUILayout.Space(2);
                if (GUILayout.Button(TimelineStyles.empty, TimelineStyles.mute)) track.SetFlag(TrackMode.Mute, false);
            }
            if (track.locked)
            {
                GUILayout.Space(2);
                if (GUILayout.Button(TimelineStyles.empty, TimelineStyles.locked)) track.SetFlag(TrackMode.Lock, false);
            }
            if (warn)
            {
                GUILayout.Space(2);
                GUILayout.Label(TimelineStyles.warn_ico, GUILayout.MaxWidth(20));
            }
            var tree = TimelineWindow.inst.tree;
            if (track.hasChilds)
            {
                GUILayout.Space(4);
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
                    emarks[i].OnGUI(RenderRect);
                }
            }
            if (track.locked)
            {
                GUI.Box(RenderRect, "", TimelineStyles.lockedBG);
            }
            OnGUIContent();
        }

        
        public void UnSelectAll(object arg)
        {
            bool selet = (bool)arg;
            TimelineWindow.inst.tree?.ResetSelect(selet);
        }

        private void AddMark(object m)
        {
            MarkAction t = (MarkAction)m;
            float time = TimelineWindow.inst.PiexlToTime(t.posX);
            EditorFactory.MakeMarker(t.type, time, track);
        }


        private void AddClip(object mpos)
        {
            Vector2 v2 = (Vector2)mpos;
            float start = TimelineWindow.inst.PiexlToTime(v2.x);
            OnAddClip(start);
            TimelineWindow.inst.Repaint();
        }
        

        private void DeleteClip(object mpos)
        {
            Vector2 pos = (Vector2)mpos;
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

        protected void DeleteTrack()
        {
            var tree = TimelineWindow.inst.tree;
            var tracks = tree.AllSelectTracks();
            if (tracks.Count > 1)
            {
                if (EditorUtility.DisplayDialog("tip", "The selected track would be deleted!", "ok", "cancel"))
                {
                    foreach (var track in tracks)
                    {
                        DeleteTrack(track);
                    }
                }
            }
            else if (track.hasChilds)
            {
                if (EditorUtility.DisplayDialog("warn", "The track contains childs, that will be deleted!", "ok",
                    "cancel"))
                {
                    var childs = tree.GetAllChilds(track);
                    foreach (var child in childs)
                    {
                        DeleteTrack(child);
                    }
                    DeleteTrack(this);
                }
            }
            else
            {
                DeleteTrack(this);
            }
        }

        private void DeleteTrack(EditorTrack etrack)
        {
            TimelineWindow.inst.tree.RmTrack(etrack);
            etrack.track.Remove(TimelineWindow.inst.timeline);
        }

        private void SetTrackFlag(TrackMode mode, bool v)
        {
            var tree = TimelineWindow.inst.tree;
            var tracks = tree.AllSelectTracks();
            if (tracks.Count > 1)
            {
                foreach (var track in tracks)
                    track.track.SetFlag(mode, v);
            }
            else
                track.SetFlag(mode, v);
            TimelineWindow.inst.Repaint();
        }


        public bool trackF;
        private EditorMark[] emarks;

        private void SetupEMarks()
        {
            int len = track.marks.Length;
            if (emarks == null || emarks.Length != len)
            {
                emarks = new EditorMark[len];
                for (int j = 0; j < len; j++)
                {
                    emarks[j] = (EditorMark)TypeUtilities.InitEObject(track.marks[j]);
                }
            }
        }

        public void OnInspector()
        {
            var c = trackColor;
            c.a = 1;
            GUILayout.BeginHorizontal();
            using (new GUIColorOverride(c))
            {
                trackF = EditorGUILayout.Foldout(trackF, trackHeader);
            }
            if (warn)
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(TimelineStyles.warn_ico);
            }
            GUILayout.EndHorizontal();
            if (trackF)
            {
                OnInspectorTrack();
                int i = 0;
                if (track.clips != null)
                {
                    foreach (var clip in track.clips)
                    {
                        EditorGUILayout.LabelField("clip" + (++i) + ": " + clip.Display, TimelineStyles.titleStyle);
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

       

        protected virtual void OnInspectorClip(IClip clip)
        {
            clip.start = EditorGUILayout.FloatField("start", clip.start);
            float d = EditorGUILayout.FloatField("duration", clip.duration);
            if (d > 0) clip.duration = d;
        }

        public virtual bool AllowClipDrag(DragMode dm, float delta, IClip c)
        {
            return true;
        }

        public virtual ClipMode CalcuteClipMode(IClip c, out float loopLen)
        {
            loopLen = 0;
            return ClipMode.None;
        }
    }
}