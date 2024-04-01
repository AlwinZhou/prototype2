using System.Collections.Generic;
using RedBjorn.Utils;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RedBjorn.SuperTiles.Settings
{
    /// <summary>
    /// Settings which contains data about helper scene names and list of gameplay LevelData
    /// </summary>
    public class LevelSettings : ScriptableObjectExtended
    {
        public string MenuSceneName;
        public string GameSceneName;
        public string LoadingSceneName;
        public float LoadingTimeMin;
        public float LoadingTimeAfter;
        public bool LoadMenuWhenLevelStarts;

        public List<LevelData> Data = new List<LevelData>();

#if UNITY_EDITOR
        public void Update()
        {
            Data.RemoveAll(p => !p);
            var guids = AssetDatabase.FindAssets(string.Concat("t:", typeof(LevelData).Name));
            var levels = new List<LevelData>();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var level = AssetDatabase.LoadAssetAtPath<LevelData>(path);
                if (!Data.Contains(level))
                {
                    Data.Add(level);
                }
            }
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
