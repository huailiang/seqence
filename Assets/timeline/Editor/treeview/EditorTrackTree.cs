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
        }

        public EditorTrack[] hierachy;

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
            List<EditorTrack> list = new List<EditorTrack>();
            for (int i = 0; i < trees.Length; i++)
            {
                Add(trees[i], list);
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
    }
}
