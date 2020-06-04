using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    public class XActiveMark : XMarker
    {
        private bool active;
        
        public XActiveMark(XTrack track, ActiveData data) : base(track, data)
        {
            active = data.active;
        }

        public override void OnTriger()
        {
            base.OnTriger();
            var bind = track as XBindTrack;
            if (bind )
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