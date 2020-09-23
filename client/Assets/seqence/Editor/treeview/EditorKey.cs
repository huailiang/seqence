using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Seqence
{
    public class EditorKey
    {
        protected readonly static GUIContent s_KeyOn = new GUIContent(SeqenceStyle.keyframe.active.background);
        protected readonly static GUIContent s_KeyOff = new GUIContent(SeqenceStyle.keyframe.normal.background);

        public float piexl, time;
        public bool select;
        public Rect rect;

        public EditorKey(float time, Rect r)
        {
            this.time = time;
            piexl = SeqenceWindow.inst.TimeToPixel(time);
            select = false;
            r.width = 8;
            r.x = piexl;
            r.y = r.y + r.height / 3;
            r.x = r.x - 4;
            rect = r;
        }

        public void SetTime(float time)
        {
            this.time = time;
            piexl = SeqenceWindow.inst.TimeToPixel(time);
            rect.x = piexl - 4;
        }

        public void Draw()
        {
            GUIContent gct = select ? s_KeyOn : s_KeyOff;
            GUI.Box(rect, gct, SeqenceStyle.keyframe);
        }


        /// <summary>
        /// build editor keys
        /// </summary>
        /// <param name="rect">clip or track rect</param>
        /// <param name="set">time set</param>
        /// <param name="keys">build result</param>
        public static void BuildEditorKeys(Rect rect, HashSet<float> set, List<EditorKey> keys)
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
                    keys[i++].SetTime(it);
                }
            }
        }


        /// <summary>
        /// build editor keys
        /// </summary>
        /// <param name="rect">clip or track rect</param>
        /// <param name="set">time set</param>
        /// <param name="keys">build result</param>
        public static void BuildEditorKeys(Rect rect, float[] set, List<EditorKey> keys)
        {
            if (set.Length != keys.Count)
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
                    keys[i++].SetTime(it);
                }
            }
        }

        public static void BuildAndDraw(Rect rect, float[] set, List<EditorKey> keys)
        {
            BuildEditorKeys(rect, set, keys);
            if (keys != null)
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    keys[i].Draw();
                }
            }
        }

    }
}