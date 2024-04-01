using RedBjorn.SuperTiles.Battle.Actions;
using RedBjorn.Utils;
using System.Collections;

namespace RedBjorn.SuperTiles.Items
{
    /// <summary>
    /// Base class for ItemAction handler with initial validation checks
    /// </summary>
    public abstract class ActionHandler : ScriptableObjectExtended
    {
        public IEnumerator Handle(ItemAction data, BattleEntity battle)
        {
            if (data == null)
            {
                Log.E("ActionHandler receive invalid data. Data is NULL.");
                yield break;
            }
            if (!data.ValidatePosition(battle))
            {
                Log.E($"ActionHandler receive invalid data. Couldn't validate position. {data}");
                yield break;
            }
            yield return DoHandle(data, battle);
        }

        public abstract IEnumerator DoHandle(ItemAction data, BattleEntity battle);

#if UNITY_EDITOR
        public static System.Type GetCreatorType(string typeName)
        {
            var type = typeof(ActionHandler);
            var creatorName = string.Concat(type.Namespace, ".", nameof(ActionHandlers), ".", typeName, "Creator");
            var creator = type.Assembly.GetType(creatorName);
            if (creator == null)
            {
                Log.W($"No creator class [{creatorName}] for ActionHandler [{typeName}]. Default will be selected");
                creator = typeof(ActionHandlers.DefaultCreator);
            }
            return creator;
        }
#endif
    }
}

