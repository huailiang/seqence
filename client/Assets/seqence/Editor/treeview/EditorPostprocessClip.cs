using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Seqence;


namespace UnityEditor.Seqence
{
    [TimelineClipEditor(typeof(XPostprocessClip))]
    public class EditorPostprocessClip : EditorClip
    {
        XPostprocessClip c
        {
            get { return clip as XPostprocessClip; }
        }

        List<EditorKey> keys = new List<EditorKey>();

        CurveBindObject curveBindObject
        {
            get { return c.curveBindObject; }
            set { c.curveBindObject = value; }
        }

        public override void PostGUI()
        {
            HashSet<float> set = curveBindObject.GetAllKeyTimes();
            if (set != null)
            {
                EditorKey.BuildEditorKeys(rect, set, keys);
                for (int i = 0; i < set.Count; i++)
                {
                    keys[i].Draw();
                }
            }
            base.PostGUI();
        }

        private void BuildEditorKeys(HashSet<float> set)
        {
            if (set.Count != keys.Count)
            {
                keys.Clear();
                foreach (var it in set)
                {
                    EditorKey ek = new EditorKey(it, rect);
                    keys.Add(ek);
                }
            }
            else
            {
                int i = 0;
                foreach (var it in set)
                {
                    keys[i++].Update(it, rect);
                }
            }
        }

        protected override bool CheckChildSelect(Vector2 pos)
        {
            if (keys != null)
            {
                float piexl = pos.x;
                foreach (var key in keys)
                {
                    if (Mathf.Abs(key.piexl - piexl) <= 2)
                    {
                        key.select = true;
                        return false;
                    }
                }
            }
            return base.CheckChildSelect(pos);
        }

        protected override void OnDrag(Event e)
        {
            float t = clip.start;
            float len = clip.duration;
            base.OnDrag(e);
            if (Mathf.Approximately(clip.duration, len))
            {
                float delta = clip.start - t;
                curveBindObject.Move(delta);
            }
            else
            {
                if (keys != null)
                {
                    float scale = clip.duration / len;
                    curveBindObject.UpdateAllKeyTimes(clip.start, scale);
                }
            }
        }
    }
}