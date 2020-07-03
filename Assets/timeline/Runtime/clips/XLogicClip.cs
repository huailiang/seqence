using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XLogicClip : XClip<XLogicTrack>
    {
        private GameObject bindObj;

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
            var bindTrack = track.root as XBindTrack;
            if (bindTrack?.bindObj)
            {
                bindObj = bindTrack.bindObj;
                DrawAttackArea();
            }
        }

        protected override void OnExit()
        {
            base.OnExit();
            TimelineDraw.Clean();
        }


        private void DrawAttackArea()
        {
            LogicClipData dta = data as LogicClipData;
            if (dta.attackShape == AttackShape.Rect)
            {
                DrawRect(dta.attackArg, dta.attackArg2);
            }
            else if (dta.attackShape == AttackShape.Ring)
            {
                DrawRing(dta.attackArg);
            }
            else if (dta.attackShape == AttackShape.Sector)
            {
                DrawSector(dta.attackArg, dta.attackArg2);
            }
        }


        private void DrawRect(float len, float width)
        {
            var obj = TimelineDraw.DrawRectangleSolid(bindObj.transform, len, width);
            timeline.BindGo(obj);
        }

        private void DrawRing(float radius)
        {
            var obj = TimelineDraw.DrawCircleSolid(bindObj.transform, radius);
            timeline.BindGo(obj);
        }

        private void DrawSector(float radius, float angle)
        {
            var obj = TimelineDraw.DrawSectorSolid(bindObj.transform, angle, radius);
            timeline.BindGo(obj);
        }
    }
}
