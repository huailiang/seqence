using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    [TimelineEditor(typeof(XLogicValueTrack))]
    public class EditorLogicTrack : EditorTrack
    {
        private int len;
        private bool[] folds;
        private const int max = (int) LogicType.MAX;

        protected override Color trackColor
        {
            get { return Color.blue; }
        }

        protected override string trackHeader
        {
            get { return "打击点" + ID; }
        }


        protected override List<TrackMenuAction> actions
        {
            get
            {
                var types = TypeUtilities.GetRootChilds(typeof(XAnimationTrack));
                List<TrackMenuAction> ret = new List<TrackMenuAction>();
                var act = new TrackMenuAction() {desc = "Delete Item", on = false, fun = DeleteItem, arg = null};
                ret.Add(act);
                return ret;
            }
        }

        private void DeleteItem(object arg)
        {
            Debug.Log("delete");
        }

        protected override void OnAddClip(float t)
        {
            LogicClipData clipData = new LogicClipData();
            clipData.start = t;
            clipData.duration = 16;

            XLogicClip clip = new XLogicClip((XLogicValueTrack) track, clipData);
            track.AddClip(clip, clipData);
        }

        protected override void OnInspectorClip(IClip c)
        {
            base.OnInspectorClip(c);
            XLogicClip clip = (XLogicClip) c;
            if (clip)
            {
                if (folds == null)
                {
                    folds = new bool[max];
                }
                LogicClipData data = clip.data as LogicClipData;
                len = data.effect?.Length ?? 0;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        folds[i] = EditorGUILayout.Foldout(folds[i], (i + 1) + ": " + data.logicType[i]);
                        EditorGUILayout.Space();
                        if (GUILayout.Button("x", TimelineStyles.bottomShadow, GUILayout.MaxWidth(20)))
                        {
                            data.effect = TimelineUtil.Remv(data.effect, i);
                            data.logicType = TimelineUtil.Remv(data.logicType, i);

                            GUIUtility.ExitGUI();
                        }
                        EditorGUILayout.EndHorizontal();
                        if (folds[i])
                        {
                            data.logicType[i] = (LogicType) EditorGUILayout.EnumPopup(" type", data.logicType[i]);
                            data.effect[i] = EditorGUILayout.FloatField(" effect", data.effect[i]);
                            if (data.logicType[i] == LogicType.MAX)
                            {
                                EditorGUILayout.HelpBox("max is attr", MessageType.Error);
                            }
                        }
                        EditorGUILayout.Space();
                    }
                }
                if (GUILayout.Button(" Add"))
                {
                    if (data.effect?.Length >= max)
                    {
                        EditorUtility.DisplayDialog("warn", "max attribute is: " + max, "ok");
                    }
                    else
                    {
                        TimelineUtil.Add(ref data.logicType, LogicType.HP);
                        TimelineUtil.Add(ref data.effect, 0.5f);
                    }
                }
                EditorGUILayout.Space();
            }
        }
    }
}
