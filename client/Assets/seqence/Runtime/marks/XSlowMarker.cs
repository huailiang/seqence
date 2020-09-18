using UnityEngine.Seqence.Data;

namespace UnityEngine.Seqence
{
    [MarkUsage(AssetType.Marker)]
    public class XSlowMarker : XMarker, ISharedObject<XSlowMarker>
    {
        private SlowMarkData _data;

        public XSlowMarker next { get; set; }

        public float slow
        {
            get { return _data.slowRate; }
            set { _data.slowRate = value; }
        }


        protected override void OnPostBuild()
        {
            base.OnPostBuild();
            _data = (SlowMarkData)Data;
        }

        public override void OnTriger()
        {
            base.OnTriger();
            seqence.slow = slow;
        }

        public override void OnDestroy()
        {
            SharedPool<XSlowMarker>.Return(this);
            base.OnDestroy();
        }

        public void Dispose()
        {
            next = null;
        }

    }
}
