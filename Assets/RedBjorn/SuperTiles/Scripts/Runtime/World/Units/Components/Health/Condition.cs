using RedBjorn.Utils;

namespace RedBjorn.SuperTiles.Health
{
    /// <summary>
    /// Base class for checking conditions on units (victim and damager) and on damage item
    /// </summary>
    public abstract class Condition : ScriptableObjectExtended
    {
        public abstract bool IsMet(float delta, UnitEntity victim, UnitEntity damager, ItemEntity item);

#if UNITY_EDITOR
        public static System.Type GetCreatorType(string typeName)
        {
            var type = typeof(Condition);
            var creatorName = string.Concat(type.Namespace, ".", nameof(Conditions), ".", typeName, "Creator");
            var creator = type.Assembly.GetType(creatorName);
            if (creator == null)
            {
                Log.W($"No creator class [{creatorName}] for ActionHandler [{typeName}]. Default will be selected");
                creator = typeof(Conditions.DefaultCreator);
            }
            return creator;
        }
#endif
    }
}
