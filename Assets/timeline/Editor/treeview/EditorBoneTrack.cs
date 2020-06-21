using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    [TimelineEditor(typeof(XBoneFxTrack))]
    public class EditorBoneTrack : EditorTrack
    {
        protected override Color trackColor
        {
            get { return Color.green; }
        }

        protected override string trackHeader
        {
            get { return "骨骼特效" + ID; }
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
                    track.AddClip(clip, data);
                }
            }, null);
        }

        protected override void OnInspectorClip(IClip c)
        {
            base.OnInspectorClip(c);
            XBoneFxClip xc = c as XBoneFxClip;
            var data = c.data as BoneFxClipData;
            data.bone = EditorGUILayout.TextField("bone", data.bone);
            EditorGUILayout.ObjectField("fx", xc.fx, typeof(GameObject), true);
        }
    }
}
