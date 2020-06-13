using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    public class EditorBoneTrack : EditorTrack
    {
        protected override Color trackColor
        {
            get { return Color.green; }
        }

        protected override void AddClip(object mpos)
        {
            ObjectSelector.get.Show(null, typeof(GameObject), null, false, null, obj =>
            {
                if (obj != null)
                {
                    Vector2 v2 = (Vector2) mpos;
                    float start = TimelineWindow.inst.PiexlToTime(v2.x);
                    BoneFxClipData data = new BoneFxClipData();
                    data.start = start;
                    data.duration = 20;
                    data.prefab = AssetDatabase.GetAssetPath(obj);
                    data.seed = 0;
                    XBoneFxClip clip = new XBoneFxClip((XBoneFxTrack) track, data);
                    clip.SetFx((GameObject) obj);
                    track.AddClip(clip);
                    base.AddClip(mpos);
                }
            }, null);
        }
    }
}
