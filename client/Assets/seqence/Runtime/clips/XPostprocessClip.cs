using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XPostprocessClip : XClip<XPostprocessTrack, XPostprocessClip>, ISharedObject<XPostprocessClip>
    {
        public override string Display
        {
            get { return "post process"; }
        }
        

        public override void OnDestroy()
        {
            SharedPool<XPostprocessClip>.Return(this);
            base.OnDestroy();
        }
        
    }
}
