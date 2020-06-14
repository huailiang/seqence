using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    [TimelineEditor(typeof(XAnimationTrack))]
    public class EditorAnimTrack : EditorTrack
    {
        protected override Color trackColor
        {
            get { return Color.yellow; }
        }

        protected override string trackHeader
        {
            get { return "角色" + ID; }
        }

        protected override List<TrackMenuAction> actions
        {
            get
            {
                var types = TypeUtilities.GetRootChilds(typeof(XAnimationTrack));
                List<TrackMenuAction> ret = new List<TrackMenuAction>();
                for (int i = 0; i < types.Count; i++)
                {
                    var str = types[i].ToString();
                    int idx = str.LastIndexOf('.');
                    if (idx >= 0)
                    {
                        str = str.Substring(idx + 1);
                    }
                    var act = new TrackMenuAction()
                    {
                        desc = "Add " + str, on = false, fun = AddSubTrack, arg = types[i]
                    };
                    ret.Add(act);
                }
                return ret;
            }
        }

        private void AddSubTrack(object arg)
        {
            Type type = (Type) arg;
            TrackData data = XTimelineFactory.CreateTrackData(type);
            var state = TimelineWindow.inst.state;
            var tr = XTimelineFactory.GetTrack(data, state.timeline);
            var tmp = track;
            if (track.childs != null)
            {
                tmp = track.childs.Last();
            }
            tr.parent = this.track;
            tr.parent.AddSub(tr);
            int idx = TimelineWindow.inst.tree.IndexOfTrack(tmp);
            TimelineWindow.inst.tree.AddTrack(tr, idx + 1);
        }


        protected override void OnGUIHeader()
        {
            XBindTrack btrack = track as XBindTrack;
            if (btrack)
            {
#pragma warning disable 618
                btrack.bindObj =
                    (GameObject) EditorGUILayout.ObjectField(btrack.bindObj, typeof(Animator), GUILayout.MaxWidth(80));
#pragma warning restore 618
            }
            EditorGUILayout.Space();
        }


        protected override void OnAddClip(float t)
        {
            ObjectSelector.get.Show(null, typeof(AnimationClip), null, false, null, obj =>
            {
                AnimationClip u_clip = (AnimationClip) obj;
                if (u_clip != null)
                {
                    AnimClipData data = new AnimClipData();
                    data.start = t;
                    data.duration = u_clip.averageDuration;
                    data.anim = AssetDatabase.GetAssetPath(u_clip);
                    data.trim_start = 0;
                    data.loop = u_clip.isLooping;
                    XAnimationTrack atr = (XAnimationTrack) track;
                    XAnimationClip clip = new XAnimationClip((XAnimationTrack) track, data);
                    clip.aclip = u_clip;
                    clip.port = track.clips?.Length ?? 0;
                    track.AddClip(clip);
                }
            }, null);
        }
    }
}
