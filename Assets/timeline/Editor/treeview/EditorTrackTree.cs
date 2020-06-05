using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public class EditorTrackTree
    {
        public struct EditorTrack
        {
            public XTrack track;
            public Rect rect;

            public void OnGUI()
            {
                var backgroundColor = TimelineStyles.markerHeaderDrawerBackgroundColor;
                EditorGUI.DrawRect(rect, backgroundColor);
            }
        }

        public List<EditorTrack> hierachy;

        private int idx = 0;
        private float x, width, _y;
        private const float height = 50;

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
            for (int i = 0; i < trees.Length; i++)
            {
                Add(trees[i], hierachy);
            }
        }

        private void Add(XTrack track, IList<EditorTrack> list)
        {
            EditorTrack etrack = new EditorTrack();
            etrack.track = track;
            float y = _y + height * idx + WindowConstants.rowGap * idx;
            etrack.rect = new Rect(x, y, width, height);
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
                    delta = height - it.rect.height;
                }
                it.rect.height += delta;
            }
        }

        public void AddTrack(XTrack track, int idx)
        {
            EditorTrack etrack = new EditorTrack();;
            etrack.track = track;
            float y = _y + height * idx + WindowConstants.rowGap * idx;
            etrack.rect = new Rect(x,y,width,height);
            hierachy.Add(etrack);
            int last = hierachy.Count - 1;
            for (int i = last; i > idx; i--)
            {
                hierachy[i] = hierachy[i - 1];
            }
            hierachy[idx] = etrack;
        }

        public void OnRmTrack(EditorTrack track)
        {
            for (int i = 0; i < hierachy.Count; i++)
            {
                var it = hierachy[i];
                float delta = 0;
                if (it.track.ID == track.track.ID)
                {
                    delta = track.rect.height + WindowConstants.rowGap;
                }
                it.rect.y -= delta;
            }
        }

        public void OnGUI()
        {
            foreach (var it in hierachy)
            {
                it.OnGUI();
            }
        }
    }
}
