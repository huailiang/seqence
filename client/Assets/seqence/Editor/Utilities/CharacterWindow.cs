using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Seqence;
using UnityEngine.Seqence.Data;
using CharacterInfo = UnityEngine.Seqence.Data.CharacterInfo;

namespace UnityEditor.Seqence
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

        public static void Load()
        {
            if (chInfo == null)
            {
                var p = SeqenceUtil.chpath;
                chInfo = AssetDatabase.LoadAssetAtPath<CharacterInfo>(p);
            }
        }

        public static Character Find(int id)
        {
            Load();
            if (chInfo != null)
            {
                for (int i = 0; i < chInfo.characters.Length; i++)
                {
                    if (chInfo.characters[i].id == id)
                    {
                        return chInfo.characters[i];
                    }
                }
            }
            return null;
        }


        protected string search;

        private void OnEnable()
        {
            Load();
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
            GUILayout.Label("Select a character", SeqenceStyle.titleStyle);
            GUILayout.Space(4);
            search = GuiSearch(search);
            var chars = chInfo.characters;
            GUILayout.BeginVertical(GUI.skin.label);
            for (int i = 0; i < chars.Length; i++)
            {
                string desc = (i + 1) + ". " + Desc(chars[i]);
                if (MatchSearch(chars[i]))
                {
                    if (GUILayout.Button(desc, SeqenceStyle.btnLableStyle))
                    {
                        character = chars[i];
                        Close();
                    }
                }
            }
            GUILayout.EndVertical();
        }


        private string Desc(Character ch)
        {
            string prefab = ch.prefab;
            if (!string.IsNullOrEmpty(prefab))
            {
                prefab = prefab.Replace(".prefab", "");
                int idx = prefab.LastIndexOf('/') + 1;
                if (idx >= 0) prefab = prefab.Substring(idx);
            }
            return ch.id + "\t" + ch.name + "\t" + prefab;
        }


        private bool MatchSearch(Character ch)
        {
            if (string.IsNullOrEmpty(search))
            {
                return true;
            }
            else
            {
                return ch.name.Contains(search) ||
                    ch.id.ToString().Contains(search);
            }
        }


        private string GuiSearch(string value, params GUILayoutOption[] options)
        {
            MethodInfo info = typeof(EditorGUILayout).GetMethod("ToolbarSearchField",
                BindingFlags.NonPublic | BindingFlags.Static, null,
                new System.Type[] { typeof(string), typeof(GUILayoutOption[]) }, null);
            if (info != null)
            {
                value = (string)info.Invoke(null, new object[] { value, options });
            }
            return value;
        }
    }
}