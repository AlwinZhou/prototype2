using RedBjorn.Utils;

namespace RedBjorn.SuperTiles.Health
{
    /// <summary>
    /// Base class for converting float value
    /// </summary>
    public abstract class ValueConverter : ScriptableObjectExtended
    {
        public abstract float Convert(float val);

#if UNITY_EDITOR
        public static System.Type GetCreatorType(string typeName)
        {
            var type = typeof(ValueConverter);
            var creatorName = string.Concat(type.Namespace, ".", nameof(ValueConverters), ".", typeName, "Creator");
            var creator = type.Assembly.GetType(creatorName);
            if (creator == null)
            {
                Log.W($"No creator class [{creatorName}] for ValueConverter [{typeName}]. Default will be selected");
                creator = typeof(ValueConverters.DefaultCreator);
            }
            return creator;
        }
#endif
    }
}
