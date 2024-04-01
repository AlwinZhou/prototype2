using RedBjorn.Utils;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Unit
{
    public class UnitWindowSettings : ScriptableObjectExtended
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

        public string DefaultItemFolder = DefaultFolder + "SuperTiles/Units";
        public string DefaultItemName = "Unit";

        public const string DefaultFolder = RedBjorn.Utils.Paths.ScriptablePath.RootFolder + "/";
        public const string DefaultPathRelative = Paths.ScriptablePath.Asset + "/Editor Resources/UnitWindowSettings.asset";
        public const string DefaultPathFull = DefaultFolder + DefaultPathRelative;

        public Color CommonColor => EditorGUIUtility.isProSkin ? Dark.CommonColor : Light.CommonColor;
        public Color MenuColor => EditorGUIUtility.isProSkin ? Dark.MenuColor : Light.MenuColor;
        public Color WorkAreaColor => EditorGUIUtility.isProSkin ? Dark.WorkAreaColor : Light.WorkAreaColor;

        public static UnitWindowSettings Instance
        {
            get
            {
                var path = DefaultPathFull;
                var instance = AssetDatabase.LoadAssetAtPath<UnitWindowSettings>(path);
                if (!instance)
                {
                    var paths = AssetDatabase.FindAssets("t:" + nameof(UnitWindowSettings))
                                             .Select(a => AssetDatabase.GUIDToAssetPath(a))
                                             .OrderBy(a => a);
                    path = paths.FirstOrDefault(i => i.Contains(DefaultPathRelative));
                    instance = AssetDatabase.LoadAssetAtPath<UnitWindowSettings>(path);
                    if (!instance)
                    {
                        path = paths.FirstOrDefault();
                        instance = AssetDatabase.LoadAssetAtPath<UnitWindowSettings>(path);
                    }
                }
                return instance;
            }
        }
    }
}
