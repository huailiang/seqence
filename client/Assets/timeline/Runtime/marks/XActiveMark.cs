using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [MarkUsage(AssetType.SceneFx | AssetType.Animation)]
    public class XActiveMark : XMarker, ISharedObject<XActiveMark>
    {
        private ActiveMarkData _data;

        public XActiveMark next { get; set; }

        public bool active
        {
            get { return _data.active; }
            set { _data.active = value; }
        }

        protected override void OnPostBuild()
        {
            base.OnPostBuild();
            _data = (ActiveMarkData)Data;
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

        public override void OnDestroy()
        {
            SharedPool<XActiveMark>.Return(this);
            base.OnDestroy();
        }

        public void Dispose()
        {
            next = null;
        }
    }
}
