using RedBjorn.Utils;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Level
{
    public class LevelWindowSettings : ScriptableObjectExtended
    {
        [Serializable]
        public class Theme
        {
            public Color CommonColor;
            public Color MenuColor;
            public Color WorkAreaColor;
        }

        public Theme Light;
        public Theme Dark;

        public string DefaultItemFolder = DefaultFolder + "SuperTiles/Levels";
        public string DefaultItemName = "Level";

        public ProtoTiles.Tiles.TileCondition TileIsItemAvailableRule;

        public const string DefaultFolder = RedBjorn.Utils.Paths.ScriptablePath.RootFolder + "/";
        public const string DefaultPathRelative = Paths.ScriptablePath.Asset + "Editor Resources/LevelWindowSettings.asset";
        public const string DefaultPathFull = DefaultFolder + DefaultPathRelative;

        public Color CommonColor => EditorGUIUtility.isProSkin ? Dark.CommonColor : Light.CommonColor;
        public Color MenuColor => EditorGUIUtility.isProSkin ? Dark.MenuColor : Light.MenuColor;
        public Color WorkAreaColor => EditorGUIUtility.isProSkin ? Dark.WorkAreaColor : Light.WorkAreaColor;

        public static LevelWindowSettings Instance
        {
            get
            {
                var path = DefaultPathFull;
                var instance = AssetDatabase.LoadAssetAtPath<LevelWindowSettings>(path);
                if (!instance)
                {
                    var paths = AssetDatabase.FindAssets("t:" + nameof(LevelWindowSettings))
                                             .Select(a => AssetDatabase.GUIDToAssetPath(a))
                                             .OrderBy(a => a);
                    path = paths.FirstOrDefault(i => i.Contains(DefaultPathRelative));
                    instance = AssetDatabase.LoadAssetAtPath<LevelWindowSettings>(path);
                    if (!instance)
                    {
                        path = paths.FirstOrDefault();
                        instance = AssetDatabase.LoadAssetAtPath<LevelWindowSettings>(path);
                    }
                }
                return instance;
            }
        }
    }
}
