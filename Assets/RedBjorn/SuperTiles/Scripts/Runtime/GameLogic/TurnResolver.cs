using RedBjorn.Utils;
using System.IO;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Base class for level rules which define how turn transit from one unit to another 
    /// </summary>
    public abstract class TurnResolver : ScriptableObjectExtended
    {
        public const string Suffix = "TurnResolver";

        /// <summary>
        /// Can unit be changed during current turn
        /// </summary>
        public abstract bool CanChangeUnit { get; }

        /// <summary>
        /// Do preparation turn actions
        /// </summary>
        /// <param name="battle"></param>
        public abstract void TurnSequenceInit(BattleEntity battle);

        /// <summary>
        /// Do main loop actions at turn start
        /// </summary>
        /// <param name="battle"></param>
        public abstract void TurnSequenceStart(BattleEntity battle);

        /// <summary>
        /// Do main loop actions at turn finish
        /// </summary>
        /// <param name="battle"></param>
        public abstract void TurnSequenceFinish(BattleEntity battle);

#if UNITY_EDITOR
        public static System.Type GetCreatorType(string typeName)
        {
            var type = typeof(TurnResolver);
            var creatorName = string.Concat(type.Namespace, ".", nameof(Resolvers), ".", typeName, "Creator");
            var creator = type.Assembly.GetType(creatorName);
            if (creator == null)
            {
                Log.W($"No creator class [{creatorName}] for TurnResolver [{typeName}]. Default will be selected");
                creator = typeof(Resolvers.DefaultCreator);
            }
            return creator;
        }

        public static TurnResolver Create(string directory, string filename, string typeName)
        {
            var creator = GetCreatorType(typeName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var turn = ((TurnResolverCreator)System.Activator.CreateInstance(creator)).Create(typeName);
            UnityEditor.EditorUtility.SetDirty(turn);
            var path = Path.Combine(directory, string.Concat(filename, "_", TurnResolver.Suffix, FileFormat.Asset));
            path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(path);
            UnityEditor.AssetDatabase.CreateAsset(turn, path);
            return turn;
        }
#endif
    }
}
