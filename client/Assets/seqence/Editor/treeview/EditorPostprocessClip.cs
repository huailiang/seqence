using System;
using System.Collections.Generic;
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

        CurveBindObject curveBindObject
        {
            get { return c.curveBindObject; }
            set { c.curveBindObject = value; }
        }

        public override void PostGUI()
        {
            HashSet<float> keys = curveBindObject.GetAllKeyTimes();
            if (keys != null)
            {
                foreach (var key in keys)
                {
                    float t = key + c.start;
                    DrawF(t, rect, false);
                }
            }
            base.PostGUI();
        }
    }
}
