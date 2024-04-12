﻿using RedBjorn.Utils;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.QuickStart
{
    public class QuickStartWindowSettings : ScriptableObjectExtended
    {
        [Serializable]
        public class Theme
        {
            public Color CommonColor;
            public Color MenuColor;
        }

        public Theme Light;
        public Theme Dark;

        public string SceneNameDefault;
        public string FolderRootDefault = RedBjorn.Utils.Paths.ScriptablePath.RootFolder + "/" + Paths.ScriptablePath.Asset + "/" + "Game/Levels";

        public ProtoTiles.Tiles.TileCondition TileIsItemAvailableRule;

        public const string DefaultPathFull = RedBjorn.Utils.Paths.ScriptablePath.RootFolder + "/" + DefaultPathRelative;
        public const string DefaultPathRelative = Paths.ScriptablePath.Asset + "Editor Resources/QuickStartWindowSettings.asset";

        public Color CommonColor => EditorGUIUtility.isProSkin ? Dark.CommonColor : Light.CommonColor;
        public Color MenuColor => EditorGUIUtility.isProSkin ? Dark.MenuColor : Light.MenuColor;

        public static QuickStartWindowSettings Instance
        {
            get
            {
                var path = DefaultPathFull;
                var instance = AssetDatabase.LoadAssetAtPath<QuickStartWindowSettings>(path);
                if (!instance)
                {
                    var paths = AssetDatabase.FindAssets("t:" + nameof(QuickStartWindowSettings))
                                             .Select(a => AssetDatabase.GUIDToAssetPath(a))
                                             .OrderBy(a => a);
                    path = paths.FirstOrDefault(i => i.Contains(DefaultPathRelative));
                    instance = AssetDatabase.LoadAssetAtPath<QuickStartWindowSettings>(path);
                    if (!instance)
                    {
                        path = paths.FirstOrDefault();
                        instance = AssetDatabase.LoadAssetAtPath<QuickStartWindowSettings>(path);
                    }
                }
                return instance;
            }
        }
    }
}