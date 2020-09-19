using UnityEngine.Seqence.Data;

namespace UnityEngine.Seqence
{
    public class XLogicClip : XClip<XLogicTrack, XLogicClip, LogicClipData>, ISharedObject<XLogicClip>
    {
        private GameObject bindObj;

        public override string Display
        {
            get
            {
                string d = "打击点 ";
                if (data.logicType?.Length > 0)
                {
                    foreach (var it in data.logicType)
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
            if (data.showShape && bindTrack?.bindObj)
            {
                bindObj = bindTrack.bindObj;
                if (data.showShape)
                {
                    DrawAttackArea();
                }
            }
        }

        protected override void OnExit()
        {
            base.OnExit();
            if (data.showShape)
            {
                track?.Clean();
            }
        }

        public override void OnDestroy()
        {
            SharedPool<XLogicClip>.Return(this);
            base.OnDestroy();
        }
        

        private void DrawAttackArea()
        {
            if (data.attackShape == AttackShape.Rect)
            {
                DrawRect(data.attackArg, data.attackArg2);
            }
            else if (data.attackShape == AttackShape.Ring)
            {
                DrawRing(data.attackArg);
            }
            else if (data.attackShape == AttackShape.Sector)
            {
                DrawSector(data.attackArg, data.attackArg2);
            }
        }


        private void DrawRect(float len, float width)
        {
            var obj = track.draw?.DrawRectangleSolid(bindObj.transform, len, width);
            seqence.BindGo(obj);
        }

        private void DrawRing(float radius)
        {
            var obj = track.draw?.DrawCircleSolid(bindObj.transform, radius);
            seqence.BindGo(obj);
        }

        private void DrawSector(float radius, float angle)
        {
            var obj = track.draw?.DrawSectorSolid(bindObj.transform, angle, radius);
            seqence.BindGo(obj);
        }
    }
}
