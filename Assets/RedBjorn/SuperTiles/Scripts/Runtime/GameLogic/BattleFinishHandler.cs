using RedBjorn.Utils;
using System;
using System.IO;

namespace RedBjorn.SuperTiles
{
    public abstract class BattleFinishHandler : ScriptableObjectExtended
    {
        public const string Suffix = "BattleFinish";

        public abstract void Handle(BattleEntity battle);

#if UNITY_EDITOR
        public static System.Type GetCreatorType(string typeName)
        {
            var type = typeof(BattleFinishHandler);
            var creatorName = string.Concat(type.Namespace, ".", nameof(SuperTiles.BattleFinish), ".", typeName, "Creator");
            var creator = type.Assembly.GetType(creatorName);
            if (creator == null)
            {
                Log.W($"No creator class [{creatorName}] for BattleFinishHandler [{typeName}]. Default will be selected");
                creator = typeof(BattleFinish.DefaultCreator);
            }
            return creator;
        }

        public static BattleFinishHandler Create(string directory, string filename, string typeName)
        {
            var creator = GetCreatorType(typeName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var handler = ((BattleFinishHandlerCreator)Activator.CreateInstance(creator)).Create(typeName);
            UnityEditor.EditorUtility.SetDirty(handler);
            var path = Path.Combine(directory, string.Concat(filename, "_", BattleFinishHandler.Suffix, FileFormat.Asset));
            path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(path);
            UnityEditor.AssetDatabase.CreateAsset(handler, path);
            return handler;
        }
#endif
    }
}
