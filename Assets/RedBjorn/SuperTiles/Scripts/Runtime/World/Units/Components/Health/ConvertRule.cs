using RedBjorn.SuperTiles.Health;
using RedBjorn.Utils;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Rule which converts input delta value if Condition is met
    /// </summary>
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Health.Rules.Rule)]
    public class ConvertRule : ScriptableObjectExtended
    {
        public Condition Condition;
        public ValueConverter Converter;

        public float Convert(float delta, UnitEntity victim, UnitEntity damager, ItemEntity item)
        {
            if (!Condition || !Converter)
            {
                return delta;
            }
            if (!Condition.IsMet(delta, victim, damager, item))
            {
                return delta;
            }
            return Converter.Convert(delta);
        }
    }
}
