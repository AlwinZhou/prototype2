using RedBjorn.SuperTiles.Battle;
using RedBjorn.Utils;
using System;
using System.IO;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Base class for level rules which define the scheme IAction should obey
    /// </summary>
    public abstract class ActionRules : ScriptableObjectExtended
    {
        public static readonly Type Item = typeof(Battle.Actions.ItemAction);
        public static readonly Type Move = typeof(Battle.Actions.MoveAction);

        public const string Suffix = "ActionRules";

        /// <summary>
        /// Is action is valid?
        /// </summary>
        /// <param name="action"></param>
        /// <param name="battle"></param>
        /// <returns></returns>
        public abstract bool Validate(BaseAction action, BattleEntity battle);

        /// <summary>
        /// Can unit make MoveAction
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="battle"></param>
        /// <returns></returns>
        public abstract bool CanMove(UnitEntity unit, BattleEntity battle);

        /// <summary>
        /// Can unit make ItemAction
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="battle"></param>
        /// <returns></returns>
        public abstract bool CanItem(UnitEntity unit, BattleEntity battle);

#if UNITY_EDITOR
        public static System.Type GetCreatorType(string typeName)
        {
            var type = typeof(ActionRules);
            var creatorName = string.Concat(type.Namespace, ".", nameof(ActionRule), ".", typeName, "Creator");
            var creator = type.Assembly.GetType(creatorName);
            if (creator == null)
            {
                Log.W($"No creator class [{creatorName}] for ActionHandler [{typeName}]. Default will be selected");
                creator = typeof(ActionRule.DefaultCreator);
            }
            return creator;
        }

        public static ActionRules Create(string directory, string filename, string typeName)
        {
            var creator = GetCreatorType(typeName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var actions = ((ActionRulesCreator)Activator.CreateInstance(creator)).Create(typeName);
            UnityEditor.EditorUtility.SetDirty(actions);
            var path = Path.Combine(directory, string.Concat(filename, "_", ActionRules.Suffix, FileFormat.Asset));
            path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(path);
            UnityEditor.AssetDatabase.CreateAsset(actions, path);
            return actions;
        }
#endif
    }
}
