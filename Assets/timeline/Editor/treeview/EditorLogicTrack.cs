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

        protected override void OnAddClip(float t)
        {
            LogicClipData clipData = new LogicClipData();
            clipData.start = t;
            clipData.duration = 16;
            folds = new bool[max];
            XLogicClip clip = new XLogicClip((XLogicValueTrack) track, clipData);
            track.AddClip(clip);
        }

        protected override void OnInspectorClip(IClip c)
        {
            base.OnInspectorClip(c);
            XLogicClip clip = (XLogicClip) c;
            if (clip)
            {
                LogicClipData data = clip.data as LogicClipData;
                if (data.effect != null)
                {
                    len = data.effect.Length;
                }
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
                EditorGUILayout.Space();
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
            }
        }
    }
}
