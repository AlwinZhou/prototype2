using RedBjorn.Utils;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Effect
{
    public class EffectWindowSettings : ScriptableObjectExtended
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

        public Sprite EffectIconDefault;
        public Material SelectorDefault;

        public string DefaultEffectFolder = DefaultFolder + "SuperTiles/Effects";
        public string DefaultEffectName = "NewEffect";
        public string OnAddedSuffix;
        public string HandlerSuffix;
        public string OnRemovedSuffix;

        public const string DefaultFolder = RedBjorn.Utils.Paths.ScriptablePath.RootFolder + "/";
        public const string DefaultPathFull = DefaultFolder + DefaultPathRelative;
        public const string DefaultPathRelative = Paths.ScriptablePath.Asset + "Editor Resources/EffectWindowSettings.asset";

        public Color CommonColor => EditorGUIUtility.isProSkin ? Dark.CommonColor : Light.CommonColor;
        public Color MenuColor => EditorGUIUtility.isProSkin ? Dark.MenuColor : Light.MenuColor;
        public Color WorkAreaColor => EditorGUIUtility.isProSkin ? Dark.WorkAreaColor : Light.WorkAreaColor;

        public static EffectWindowSettings Instance
        {
            get
            {
                var path = DefaultPathFull;
                var instance = AssetDatabase.LoadAssetAtPath<EffectWindowSettings>(path);
                if (!instance)
                {
                    var paths = AssetDatabase.FindAssets("t:" + nameof(EffectWindowSettings))
                                             .Select(a => AssetDatabase.GUIDToAssetPath(a))
                                             .OrderBy(a => a);
                    path = paths.FirstOrDefault(i => i.Contains(DefaultPathRelative));
                    instance = AssetDatabase.LoadAssetAtPath<EffectWindowSettings>(path);
                    if (!instance)
                    {
                        path = paths.FirstOrDefault();
                        instance = AssetDatabase.LoadAssetAtPath<EffectWindowSettings>(path);
                    }
                }
                return instance;
            }
        }
    }
}
