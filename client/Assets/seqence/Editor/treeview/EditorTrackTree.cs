using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Seqence;

namespace UnityEditor.Seqence
{
    public class EditorTrackTree
    {
        public List<EditorTrack> hierachy;
        private int track_idx;
        private float x, width, _y;

        private Vector2 scroll;
        private Rect posRect, viewRect, winRect;

        public float TracksBtmY
        {
            get
            {
                if (hierachy != null && hierachy.Count > 0)
                {
                    var track = hierachy.Last();
                    return track.rect.y + track.rect.height;
                }
                return WindowConstants.markerRowYPosition;
            }
        }

        public void Dispose()
        {
            hierachy?.Clear();
            track_idx = 0;
            hierachy = null;
        }

        public void BuildTreeHierachy(SeqenceState state)
        {
            if (state.seqence == null)
            {
                throw new Exception("seqence is null");
            }
            winRect = state.window.winArea;
            x = WindowConstants.rightAreaMargn;
            _y = WindowConstants.trackRowYPosition;
            width = winRect.width;
            track_idx = 0;
            var trees = state.seqence.trackTrees;
            if (trees != null)
            {
                hierachy = new List<EditorTrack>();
                for (int i = 1; i < trees.Length; i++) // 0 is marker track
                {
                    Add(trees[i], hierachy);
                }
            }
            BuildSkillHost();
        }

        private void Add(XTrack track, IList<EditorTrack> list)
        {
            EditorTrack etrack = EditorFactory.GetTrack(track);
            float y = _y + WindowConstants.RawHeight * track_idx + WindowConstants.rowGap * track_idx;
            int offset = track.parent ? 10 : 0;
            var rect = new Rect(x, y, width, WindowConstants.RawHeight);
            var head = new Rect(offset, y, WindowConstants.sliderWidth - offset, WindowConstants.RawHeight);
            etrack.SetRect(head, rect);
            track_idx++;
            list.Add(etrack);
            if (track.childs != null)
            {
                for (int i = 0; i < track.childs.Length; i++)
                {
                    Add(track.childs[i], list);
                }
            }
        }

        public List<EditorTrack> GetAllChilds(XTrack track, bool grandsonContains = true)
        {
            List<EditorTrack> list = new List<EditorTrack>();
            for (int i = 0; i < hierachy.Count; i++)
            {
                var it = hierachy[i];
                if (it.track.IsChild(track, grandsonContains))
                {
                    list.Add(it);
                }
            }
            return list;
        }

        public int IndexOfTrack(XTrack track)
        {
            for (int i = 0; i < hierachy.Count; i++)
            {
                var it = hierachy[i];
                if (it.ID == track.ID) return i;
            }
            return 0;
        }

        public void AddTrack(XTrack track, object arg = null)
        {
            AddTrack(track, hierachy.Count, arg);
        }

        public void AddTrack(XTrack track, int idx, object arg = null, bool repaint = true)
        {
            EditorTrack etrack = EditorFactory.GetTrack(track);
            etrack.trackArg = arg;
            float y = _y + WindowConstants.RawHeight * idx + WindowConstants.rowGap * idx;
            float offset = track.parent ? 10 : 0;
            var rect = new Rect(x, y, width, WindowConstants.RawHeight);
            var head = new Rect(offset, y, WindowConstants.sliderWidth - offset, WindowConstants.RawHeight);
            etrack.SetRect(head, rect);
            hierachy.Add(etrack);
            int last = hierachy.Count - 1;
            for (int i = last; i > idx; i--)
            {
                hierachy[i] = hierachy[i - 1];
                hierachy[i].YOffset(WindowConstants.RawHeight + WindowConstants.rowGap);
            }
            hierachy[idx] = etrack;
            if (repaint) SeqenceWindow.inst.Repaint();
        }

        public void AddChildTracks(XTrack track)
        {
            var childs = track.childs;
            int ix = IndexOfTrack(track);
            if (childs != null)
            {
                for (int i = 0; i < track.childs.Length; i++)
                {
                    AddTrack(track.childs[i], ++ix, false);
                }
                SeqenceWindow.inst.Repaint();
            }
        }

        public void RmChildTrack(XTrack track)
        {
            List<EditorTrack> list = new List<EditorTrack>();
            for (int i = 0; i < hierachy.Count; i++)
            {
                if (hierachy[i].track.parent.Equals(track))
                {
                    list.Add(hierachy[i]);
                }
            }
            foreach (var editorTrack in list)
            {
                RmTrack(editorTrack, false);
            }
            SeqenceWindow.inst.Repaint();
        }

        public void RmTrack(EditorTrack track, bool repaint = true)
        {
            int ix = -1;
            float delta = 0;
            for (int i = 0; i < hierachy.Count; i++)
            {
                var it = hierachy[i];
                if (it.ID == track.track.ID)
                {
                    ix = i;
                    delta = track.rect.height + WindowConstants.rowGap;
                }
                it.YOffset(-delta);
            }
            if (ix >= 0)
            {
                hierachy.RemoveAt(ix);
            }
            if (repaint) SeqenceWindow.inst.Repaint();
        }

        private void SyncTreeWidth()
        {
            if (hierachy != null)
                for (int i = 0; i < hierachy.Count; i++)
                {
                    hierachy[i].rect.width = width;
                }
        }

        public void OnGUI(SeqenceState state)
        {
            if (hierachy == null)
            {
                BuildTreeHierachy(state);
            }
            winRect = SeqenceWindow.inst.winArea;
            posRect = winRect;
            posRect.x = x;
            posRect.y = _y;
            posRect.height = winRect.height - _y;
            posRect.width = winRect.width - x;
            viewRect = posRect;
            viewRect.height = TracksBtmY - _y;
            viewRect.width -= 20;
            width = winRect.width;
            SyncTreeWidth();

            float y = WindowConstants.trackRowYPosition;
            Rect clip = new Rect(0, y, winRect.width, winRect.height - y);

            GUI.BeginClip(clip);
            if (hierachy != null)
                for (int i = 0; i < hierachy.Count; i++)
                {
                    hierachy[i].OnGUI(scroll);
                }
            GUI.EndClip();
            bool vshow = viewRect.height > posRect.height;
            if (vshow)
            {
                scroll = GUI.BeginScrollView(posRect, scroll, viewRect, false, vshow);
                GUI.EndScrollView();
            }
        }

        public void ResetSelect(object arg)
        {
            bool select = (bool) arg;
            var d = new EventSelectData();
            d.@select = select;
            EventMgr.Send(d);
            SeqenceWindow.inst.Repaint();
        }

        public bool AnySelect()
        {
            if (hierachy != null)
            {
                return hierachy.Any(x => x.@select);
            }
            return false;
        }

        public List<EditorTrack> AllSelectTracks()
        {
            var ret = new List<EditorTrack>();
            if (hierachy != null)
            {
                for (int i = hierachy.Count - 1; i >= 0; i--)
                {
                    var it = hierachy[i];
                    if (it.select) ret.Add(it);
                }
            }
            return ret;
        }

        public void ShiftSelects(EditorTrack track)
        {
            int ix2 = -1;
            for (int i = 0; i < hierachy.Count; i++)
            {
                if (hierachy[i].select)
                {
                    ix2 = i;
                    break;
                }
            }
            ResetSelect(false);
            if (ix2 < 0)
            {
                track.select = true;
            }
            else
            {
                var ix1 = IndexOfTrack(track.track);
                int min = Mathf.Min(ix1, ix2);
                int len = Mathf.Abs(ix1 - ix2) + 1;
                for (int i = 0; i < len; i++)
                {
                    hierachy[min + i].select = true;
                }
            }
        }

        public void SetSelect(EditorTrack track)
        {
            hierachy?.ForEach(x => x.trackF = x.Equals(track));
        }

        public void SetSkillhost(EditorAnimTrack track)
        {
            var timeline = SeqenceWindow.inst.timeline;
            hierachy.ForEach(x => x.isSkillHost = x == track);
            int idx = Array.IndexOf(timeline.trackTrees, track);
            if (idx > 0)
            {
                timeline.config.skillHostTrack = (ushort) idx;
                timeline.SkillHostTrack = track.track;
            }
        }
        
        private void BuildSkillHost()
        {
            var timeline = SeqenceWindow.inst.timeline;
            int idx = timeline.config.skillHostTrack;
            if (idx > 0 && idx < timeline.trackTrees.Length)
            {
                var track = timeline.trackTrees[idx];
                hierachy.ForEach(x => x.isSkillHost = x == track);
            }
        }
    }
}
