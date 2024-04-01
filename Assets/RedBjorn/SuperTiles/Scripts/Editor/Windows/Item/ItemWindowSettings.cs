using RedBjorn.Utils;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Item
{
    public class ItemWindowSettings : ScriptableObjectExtended
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

        public Sprite ItemIconDefault;
        public Material SelectorDefault;
        public Material AvailableDefault;
        public Material TrajectoryDefault;

        public string DefaultItemFolder = DefaultFolder + "SuperTiles/Items";
        public string DefaultItemName = "NewItem";
        public string HandlerItemSuffix = "ActionHandler";
        public string TargetSelectorItemSuffix = "TargetSelector";

        public const string DefaultFolder = RedBjorn.Utils.Paths.ScriptablePath.RootFolder + "/";
        public const string DefaultPathFull = DefaultFolder + DefaultPathRelative;
        public const string DefaultPathRelative = Paths.ScriptablePath.Asset + "Editor Resources/ItemWindowSettings.asset";

        public Color CommonColor => EditorGUIUtility.isProSkin ? Dark.CommonColor : Light.CommonColor;
        public Color MenuColor => EditorGUIUtility.isProSkin ? Dark.MenuColor : Light.MenuColor;
        public Color WorkAreaColor => EditorGUIUtility.isProSkin ? Dark.WorkAreaColor : Light.WorkAreaColor;

        public static ItemWindowSettings Instance
        {
            get
            {
                var path = DefaultPathFull;
                var instance = AssetDatabase.LoadAssetAtPath<ItemWindowSettings>(path);
                if (!instance)
                {
                    var paths = AssetDatabase.FindAssets("t:" + nameof(ItemWindowSettings))
                                             .Select(a => AssetDatabase.GUIDToAssetPath(a))
                                             .OrderBy(a => a);
                    path = paths.FirstOrDefault(i => i.Contains(DefaultPathRelative));
                    instance = AssetDatabase.LoadAssetAtPath<ItemWindowSettings>(path);
                    if (!instance)
                    {
                        path = paths.FirstOrDefault();
                        instance = AssetDatabase.LoadAssetAtPath<ItemWindowSettings>(path);
                    }
                }
                return instance;
            }
        }
    }
}
