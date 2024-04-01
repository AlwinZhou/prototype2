using RedBjorn.SuperTiles.Battle;
using RedBjorn.SuperTiles.Squad;
using RedBjorn.Utils;
using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Base class for Ai logic controlling single unit 
    /// </summary>
    public abstract class UnitAiData : ScriptableObjectExtended
    {
        public abstract bool TryNextAction(AiEntity player, UnitAiEntity ai, BattleEntity battle, out BaseAction action);

        public static UnitAiData FindDefault()
        {
#if UNITY_EDITOR
            var guids = AssetDatabase.FindAssets(string.Concat("t:", typeof(UnitAiData).Name, " ", "Default"));
            if (guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                var data = AssetDatabase.LoadAssetAtPath<UnitAiData>(path);
                return data;
            }

            guids = AssetDatabase.FindAssets(string.Concat("t:", typeof(UnitAiData).Name));
            if (guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                var data = AssetDatabase.LoadAssetAtPath<UnitAiData>(path);
                return data;
            }
#endif
            return null;
        }

#if UNITY_EDITOR
        public static UnitAiData Create(string folderPath, string name, string type)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var path = Path.Combine(folderPath, string.Concat(name, FileFormat.Asset));
                path = AssetDatabase.GenerateUniqueAssetPath(path);
                var ai = ScriptableObject.CreateInstance(type) as UnitAiData;

                AssetDatabase.CreateAsset(ai, path);
                EditorUtility.SetDirty(ai);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Log.I($"New unit ai was created at path: {path}");

                return ai;
            }
            catch (Exception e)
            {
                Log.E(e);
                return null;
            }
        }
#endif
    }
}