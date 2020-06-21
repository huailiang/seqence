using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [MarkUsage(AssetType.SceneFx | AssetType.Animation)]
    public class XActiveMark : XMarker
    {
        private ActiveMarkData _data;

        public bool active
        {
            get { return _data.active; }
            set { _data.active = value; }
        }

        public XActiveMark(XTrack track, MarkData markData) : base(track, markData)
        {
            _data = (ActiveMarkData)markData;
        }

        public override void OnTriger()
        {
            base.OnTriger();
            var bind = track as XBindTrack;
            if (bind)
            {
                var go = bind.bindObj;
                if (go)
                {
                    go.SetActive(active);
                }
            }
        }
    }
}
