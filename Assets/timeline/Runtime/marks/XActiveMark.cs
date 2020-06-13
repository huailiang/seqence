using UnityEngine.Timeline.Data;

namespace UnityEngine.Timeline
{
    [MarkUsage(TrackType.SceneFx | TrackType.Animation)]
    public class XActiveMark : XMarker
    {
        private bool active;

        public XActiveMark(XTrack track, ActiveMarkData markData) : base(track, markData)
        {
            active = markData.active;
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
