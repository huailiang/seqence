using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    [TimelineEditor(typeof(XLogicTrack))]
    public class EditorLogicTrack : EditorTrack
    {
        private int len;
        private bool[] folds;
        private const int max = (int)LogicType.MAX;

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

            var clip = track.BuildClip(clipData);
            track.AddClip(clip, clipData);
        }


        protected override void OnInspectorClip(IClip c)
        {
            base.OnInspectorClip(c);
            XLogicClip clip = (XLogicClip)c;
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
                    data.showShape = EditorGUILayout.Toggle("Show Attack Atea", data.showShape);
                    if (data.showShape)
                    {
                        data.attackShape = (AttackShape)EditorGUILayout.EnumPopup("Shape", data.attackShape);
                        if (data.attackShape == AttackShape.Rect)
                        {
                            data.attackArg = EditorGUILayout.FloatField("length: ", data.attackArg);
                            data.attackArg2 = EditorGUILayout.FloatField("width: ", data.attackArg2);
                        }
                        else if (data.attackShape == AttackShape.Ring)
                        {
                            data.attackArg = EditorGUILayout.FloatField("radius: ", data.attackArg);
                        }
                        else if (data.attackShape == AttackShape.Sector)
                        {
                            data.attackArg = EditorGUILayout.FloatField("radius: ", data.attackArg);
                            data.attackArg2 = EditorGUILayout.FloatField("angle: ", data.attackArg2);
                        }
                    }
                    for (int i = 0; i < len; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        folds[i] = EditorGUILayout.Foldout(folds[i], (i + 1) + ": " + data.logicType[i]);
                        EditorGUILayout.Space();
                        if (GUILayout.Button("x", SeqenceStyle.bottomShadow, GUILayout.MaxWidth(20)))
                        {
                            data.effect = SeqenceUtil.Remv(data.effect, i);
                            data.logicType = SeqenceUtil.Remv(data.logicType, i);
                            GUIUtility.ExitGUI();
                        }
                        EditorGUILayout.EndHorizontal();
                        if (folds[i])
                        {
                            data.logicType[i] = (LogicType)EditorGUILayout.EnumPopup(" type", data.logicType[i]);
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
                        SeqenceUtil.Add(ref data.logicType, LogicType.HP);
                        SeqenceUtil.Add(ref data.effect, 0.5f);
                    }
                }
                EditorGUILayout.Space();
            }
        }
    }
}
