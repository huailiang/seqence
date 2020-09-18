using System;

namespace UnityEngine.Seqence.Data
{
    [Serializable]
    public class Character
    {
        public int id;
        public string name;
        public string prefab;
    }

    [CreateAssetMenu(menuName = "Create CharacterInfo", fileName = "CharacterInfo", order = 0)]
    public class CharacterInfo : ScriptableObject
    {
        public Character[] characters;
    }
}
