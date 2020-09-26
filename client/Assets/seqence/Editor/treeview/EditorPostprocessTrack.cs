using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Seqence;
using UnityEngine.Seqence.Data;

namespace UnityEditor.Seqence
{
    [SeqenceEditor(typeof(XPostprocessTrack))]
    public class EditorPostprocessTrack : RecordTrack
    {
        protected override Color trackColor
        {
            get { return Color.magenta; }
        }

        protected override string trackHeader
        {
            get { return "后处理" + ID; }
        }

        protected override bool warn
        {
            get { return track.clips == null || Camera.main == null; }
        }

        protected override GameObject target
        {
            get
            {
                var camera = Camera.main;
                if (camera) return camera.gameObject;
                return null;
            }
        }

        protected override void OnAddClip(float t)
        {
            PostprocessData data = new PostprocessData();
            data.start = t;
            data.duration = 8;
            var clip = track.BuildClip(data);
            track.AddClip(clip, data);
        }

        protected override void OnInspectorTrack()
        {
            base.OnInspectorTrack();
            var c = Camera.main;
            if (c == null)
            {
                EditorGUILayout.HelpBox("Not found main camera", MessageType.Warning);
            }
            else
            {
                var layer = c.gameObject.GetComponent<PostProcessLayer>();
                if (layer == null)
                    EditorGUILayout.HelpBox("post process layer in main camera", MessageType.Warning);
                else if (track.clips == null)
                {
                    EditorGUILayout.HelpBox("There is no clip in track", MessageType.Warning);
                }
            }
        }

        protected override void OnInspectorClip(IClip clip)
        {
            base.OnInspectorClip(clip);
            XPostprocessClip postClip = clip as XPostprocessClip;
            postClip?.OnInspector(SeqenceWindow.inst.Repaint);
        }
    }
}
