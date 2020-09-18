using UnityEngine;
using UnityEngine.Seqence;
using UnityEngine.Seqence.Data;

namespace UnityEditor.Seqence
{
    [TimelineEditor(typeof(XPostprocessTrack))]
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
            get { return track.clips == null; }
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
            if (track.clips == null)
            {
                EditorGUILayout.HelpBox("There is no clip in track", MessageType.Warning);
            }
        }

        protected override void OnInspectorClip(IClip clip)
        {
            base.OnInspectorClip(clip);
            XPostprocessClip postClip = clip as XPostprocessClip;
            postClip?.OnInspector();
        }

        protected override void KeyFrame(Vector2 pos)
        {
            float t = SeqenceWindow.inst.PiexlToTime(pos.x);
            foreach (var clip in track.clips)
            {
                if (clip.start < t && t < clip.end)
                {

                }
            }
        }

        protected override void DeleteFrame(Vector2 mouse)
        {
            base.DeleteFrame(mouse);
        }

    }

}
