using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

namespace UnityEditor.Timeline
{
    [System.Serializable]
    public class MarkConfig
    {
        public MarkType type;
        public Texture2D ico;
        private GUIContent content;

        public MarkConfig()
        {
            type = MarkType.Active;
        }

        private bool folder;

        public GUIContent Content
        {
            get { return new GUIContent(ico, type + "."); }
        }

        public void OnGUI()
        {
            folder = EditorGUILayout.Foldout(folder, type.ToString());
            if (folder)
            {
                type = (MarkType) EditorGUILayout.EnumPopup("type: ", type);
                ico = (Texture2D) EditorGUILayout.ObjectField("ico: ", ico, typeof(Texture2D), false);
                GUILayout.Space(4);
            }
        }
    }


    public class AssetConfig : ScriptableObject
    {
        public MarkConfig[] marks;

        public GUIContent GetIcon(MarkType type)
        {
            return marks.Where(x => x.type == type).Select(x => x.Content).First();
        }

        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    [CustomEditor(typeof(AssetConfig))]
    public sealed class AssetConfigEditor : Editor
    {
        private AssetConfig conf;

        internal const string path = "Assets/seqence/Editor/StyleSheets/conf.asset";

        // [MenuItem("Assets/TimelineConf")]
        static void CreateAsset()
        {
            if (!File.Exists(path))
            {
                AssetConfig od = ScriptableObject.CreateInstance<AssetConfig>();
                AssetDatabase.CreateAsset(od, path);
                AssetDatabase.ImportAsset(path);
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("The asset has exist " + path);
            }
        }

        private void OnEnable()
        {
            conf = target as AssetConfig;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical();

            GuiButtons();
            GUILayout.EndVertical();
        }

        private void GuiButtons()
        {
            GUILayout.Space(8);

            GUILayout.BeginVertical();
            if (conf.marks != null)
            {
                foreach (var mark in conf.marks)
                {
                    mark.OnGUI();
                }
            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("AddMark"))
            {
                AddMark();
            }
            if (GUILayout.Button("Save"))
            {
                OnSave();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private bool addmark;

        private void AddMark()
        {
            MarkConfig config = new MarkConfig();
            SeqenceUtil.Add(ref conf.marks, config);
        }

        private void OnSave()
        {
            if (conf != null)
            {
                conf.Save();
            }
        }
    }
}
