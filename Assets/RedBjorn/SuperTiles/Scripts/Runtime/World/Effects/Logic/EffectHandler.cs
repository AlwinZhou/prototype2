using RedBjorn.Utils;
using System.Collections;

namespace RedBjorn.SuperTiles.Effects
{
    /// <summary>
    /// Base class for EffectHandler with initial validation checks
    /// </summary>
    public abstract class EffectHandler : ScriptableObjectExtended
    {
        public IEnumerator Handle(EffectEntity effect, UnitEntity unit, BattleEntity battle)
        {
            yield return DoHandle(effect, unit, battle);
        }

        protected abstract IEnumerator DoHandle(EffectEntity effect, UnitEntity unit, BattleEntity battle);

#if UNITY_EDITOR
        public static System.Type GetCreatorType(string typeName)
        {
            var type = typeof(EffectHandler);
            var creatorName = string.Concat(type.Namespace, ".", nameof(Handlers), ".", typeName, "Creator");
            var creator = type.Assembly.GetType(creatorName);
            if (creator == null)
            {
                Log.W($"No creator class [{creatorName}] for EffectHandler [{typeName}]. Default will be selected");
                creator = typeof(Handlers.DefaultCreator);
            }
            return creator;
        }
#endif
    }
}
