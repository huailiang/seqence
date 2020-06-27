using System;
using UnityEngine;
using UnityEngine.Timeline.Data;
using CharacterInfo = UnityEngine.Timeline.Data.CharacterInfo;

namespace UnityEditor.Timeline
{
    public class CharacterWindow : EditorWindow
    {
        private static CharacterInfo chInfo;
        private static Character character;
        private static Action<Character> callback;

        public static void ShowWindow(Action<Character> cb)
        {
            callback = cb;
            var window = GetWindow<CharacterWindow>();
            window.titleContent = new GUIContent("select character");
            window.Show();
        }


        private void OnEnable()
        {
            if (chInfo == null)
            {
                var p = "Assets/timeline/Editor/StyleSheets/CharacterInfo.asset";
                chInfo = AssetDatabase.LoadAssetAtPath<CharacterInfo>(p);
            }
            character = null;
        }

        private void OnLostFocus()
        {
            callback?.Invoke(character);
            callback = null;
        }

        private void OnDestroy()
        {
            callback?.Invoke(character);
            callback = null;
        }


        private void OnGUI()
        {
            var chars = chInfo.characters;
            GUILayout.BeginVertical(GUI.skin.label);
            for (int i = 0; i < chars.Length; i++)
            {
                string desc = chars[i].name + " " + chars[i].id;
                if (GUILayout.Button(desc))
                {
                    character = chars[i];
                    Close();
                }
            }
            GUILayout.EndVertical();
        }
    }
}
