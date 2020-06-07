using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public class EditorTrackTree
    {
        public List<EditorTrack> hierachy;

        private int idx = 0;
        private float x, width, _y;
        private const float height = 40;

        public float TracksBtmY
        {
            get
            {
                if (hierachy != null && hierachy.Count > 0)
                {
                    var track = hierachy.Last();
                    return track.rect.y + track.rect.height / 2;
                }
                return WindowConstants.markerRowYPosition;
            }
        }

        public XTrack GetSelectTrack()
        {
            if (hierachy != null && hierachy.Count > 0)
            {
                var list = hierachy.Where(x => x.select);
                var editorTracks = list as EditorTrack[] ?? list.ToArray();
                return editorTracks.Any() ? editorTracks.Select(x => x.track).Last() : null;
            }
            return null;
        }

        public void BuildTreeHierachy(TimelineState state)
        {
            if (state.timeline == null)
            {
                throw new Exception("timeline is null");
            }
            var winArea = state.window.winArea;
            x = winArea.x + WindowConstants.rightAreaMargn;
            _y = WindowConstants.trackRowYPosition;
            width = winArea.width;
            idx = 0;
            var trees = state.timeline.trackTrees;
            hierachy = new List<EditorTrack>();
            for (int i = 1; i < trees.Length; i++) // 0 is marker track
            {
                Add(trees[i], hierachy);
            }
        }

        public void MarksOffset(bool show)
        {
            float delta = WindowConstants.markerRowHeight;
            if (!show) delta = -delta;
            if (hierachy != null)
            {
                foreach (var it in hierachy)
                {
                    it.YOffset(delta);
                }
            }
        }

        private void Add(XTrack track, IList<EditorTrack> list)
        {
            EditorTrack etrack = EditorFactory.GetTrack(track);
            float y = _y + height * idx + WindowConstants.rowGap * idx;
            int offset = track.parent ? 10 : 0;
            etrack.rect = new Rect(x, y, width, height);
            etrack.head = new Rect(offset, y, WindowConstants.sliderWidth - offset, height);

            idx++;
            list.Add(etrack);
            if (track.childs != null)
            {
                for (int i = 0; i < track.childs.Length; i++)
                {
                    Add(track.childs[i], list);
                }
            }
        }

        public void OnTrackHeightChange(EditorTrack track, float height)
        {
            for (int i = 0; i < hierachy.Count; i++)
            {
                var it = hierachy[i];
                float delta = 0;
                if (it.track.ID == track.track.ID)
                {
                    it.SetHeight(height);
                    delta = height - it.rect.height;
                }
                it.YOffset(delta);
            }
        }

        public int IndexOfTrack(XTrack track)
        {
            for (int i = 0; i < hierachy.Count; i++)
            {
                var it = hierachy[i];
                if (it.ID == track.ID)
                {
                    return i;
                }
            }
            return 0;
        }

        public void AddTrack(XTrack track)
        {
            AddTrack(track, hierachy.Count);
        }

        public void AddTrack(XTrack track, int idx)
        {
            EditorTrack etrack = EditorFactory.GetTrack(track);
            float y = _y + height * idx + WindowConstants.rowGap * idx;
            float offset = track.parent ? 10 : 0;
            etrack.rect = new Rect(x, y, width, height);
            etrack.head = new Rect(offset, y, WindowConstants.sliderWidth - offset, height);
            hierachy.Add(etrack);
            int last = hierachy.Count - 1;
            for (int i = last; i > idx; i--)
            {
                hierachy[i] = hierachy[i - 1];
                hierachy[i].YOffset(height + WindowConstants.rowGap);
            }
            hierachy[idx] = etrack;
            TimelineWindow.inst.Repaint();
        }

        public void OnRmTrack(EditorTrack track)
        {
            for (int i = 0; i < hierachy.Count; i++)
            {
                var it = hierachy[i];
                float delta = 0;
                if (it.ID == track.track.ID)
                {
                    delta = track.rect.height + WindowConstants.rowGap;
                }
                it.YOffset(-delta);
            }
            TimelineWindow.inst.Repaint();
        }

        public void OnGUI(TimelineState state)
        {
            if (hierachy == null)
            {
                BuildTreeHierachy(state);
            }
            foreach (var it in hierachy)
            {
                it.OnGUI();
            }
        }

        public void ResetSelect()
        {
            if (hierachy != null)
                foreach (var iTrack in hierachy)
                {
                    iTrack.@select = false;
                }
            TimelineWindow.inst.Repaint();
        }

        public bool AnySelect()
        {
            if (hierachy != null)
            {
                foreach (var it in hierachy)
                {
                    if (it.@select)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
