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

        protected override void OnAddClip(float t)
        {
            ObjectSelector.get.Show(null, typeof(GameObject), null, false, null, obj =>
            {
                if (obj != null)
                {
                    BoneFxClipData data = new BoneFxClipData();
                    data.start = t;
                    data.duration = 20;
                    data.prefab = AssetDatabase.GetAssetPath(obj);
                    data.seed = 0;
                    XBoneFxClip clip = new XBoneFxClip((XBoneFxTrack) track, data);
                    clip.SetFx((GameObject) obj);
                    track.AddClip(clip);
                }
            }, null);
        }
    }
}
