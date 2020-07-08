using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XPostprocessClip : XClip<XPostprocessTrack>, ISharedObject<XPostprocessClip>
    {
        public override string Display
        {
            get { return "post process"; }
        }

        public XPostprocessClip next { get; set; }

        public override void OnDestroy()
        {
            SharedPool<XPostprocessClip>.Return(this);
            base.OnDestroy();
        }

        public void Dispose()
        {
            next = null;
        }

    }
}
