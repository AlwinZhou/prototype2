using RedBjorn.Utils;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.HealthRule
{
    public class HealthRuleWindowSettings : ScriptableObjectExtended
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

        public string DefaultRuleFolder = DefaultFolder + "SuperTiles/Rules";
        public string DefaultRuleName = "NewRule";
        public string ConverterSuffix = "Converter";
        public string ConditionSuffix = "Condition";

        public const string DefaultFolder = RedBjorn.Utils.Paths.ScriptablePath.RootFolder + "/";
        public const string DefaultPathFull = DefaultFolder + DefaultPathRelative;
        public const string DefaultPathRelative = Paths.ScriptablePath.Asset + "Editor Resources/HealthRuleWindowSettings.asset";

        public Color CommonColor => EditorGUIUtility.isProSkin ? Dark.CommonColor : Light.CommonColor;
        public Color MenuColor => EditorGUIUtility.isProSkin ? Dark.MenuColor : Light.MenuColor;
        public Color WorkAreaColor => EditorGUIUtility.isProSkin ? Dark.WorkAreaColor : Light.WorkAreaColor;

        public static HealthRuleWindowSettings Instance
        {
            get
            {
                var path = DefaultPathFull;
                var instance = AssetDatabase.LoadAssetAtPath<HealthRuleWindowSettings>(path);
                if (!instance)
                {
                    var paths = AssetDatabase.FindAssets("t:" + nameof(HealthRuleWindowSettings))
                                             .Select(a => AssetDatabase.GUIDToAssetPath(a))
                                             .OrderBy(a => a);
                    path = paths.FirstOrDefault(i => i.Contains(DefaultPathRelative));
                    instance = AssetDatabase.LoadAssetAtPath<HealthRuleWindowSettings>(path);
                    if (!instance)
                    {
                        path = paths.FirstOrDefault();
                        instance = AssetDatabase.LoadAssetAtPath<HealthRuleWindowSettings>(path);
                    }
                }
                return instance;
            }
        }
    }
}
