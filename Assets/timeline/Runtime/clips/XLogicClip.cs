using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XLogicClip : XClip<XLogicTrack>
    {
        public XLogicClip(XLogicTrack track, ClipData data) : base(track, data)
        {
        }

        public override string Display
        {
            get
            {
                LogicClipData dta = data as LogicClipData;
                string d = "打击点 ";
                if (dta.logicType?.Length > 0)
                {
                    foreach (var it in dta.logicType)
                    {
                        d += it + ",";
                    }
                    if (d.EndsWith(","))
                    {
                        d = d.Remove(d.Length - 1);
                    }
                }
                return d;
            }
        }
        

        protected override void OnEnter()
        {
            base.OnEnter();
        }

        protected override void OnExit()
        {
            base.OnExit();
        }
    }
}
