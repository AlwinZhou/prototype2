using UnityEngine;

namespace RedBjorn.SuperTiles.Health
{
    /// <summary>
    /// Interface for ValueConverter creation from string type
    /// </summary>
    public interface ValueConverterCreator
    {
        ValueConverter Create(ConvertRule rule, string type);
    }

    namespace ValueConverters
    {
        /// <summary>
        /// Default creator for ValueConverter with initial values
        /// </summary>
        public class DefaultCreator : ValueConverterCreator
        {
            public ValueConverter Create(ConvertRule rule, string type)
            {
                return ScriptableObject.CreateInstance(type) as ValueConverter;
            }
        }

        /// <summary>
        /// Creator class for Add ValueConverter
        /// </summary>
        public class AddCreator : ValueConverterCreator
        {
            public ValueConverter Create(ConvertRule rule, string type)
            {
                var converter = ScriptableObject.CreateInstance(type) as Add;
                converter.Value = 10;
                return converter;
            }
        }

        /// <summary>
        /// Creator class for Substract ValueConverter
        /// </summary>
        public class SubstractCreator : ValueConverterCreator
        {
            public ValueConverter Create(ConvertRule rule, string type)
            {
                var converter = ScriptableObject.CreateInstance(type) as Substract;
                converter.Value = 10;
                return converter;
            }
        }

        /// <summary>
        /// Creator class for Multiply ValueConverter
        /// </summary>
        public class MultiplyCreator : ValueConverterCreator
        {
            public ValueConverter Create(ConvertRule rule, string type)
            {
                var converter = ScriptableObject.CreateInstance(type) as Multiply;
                converter.Value = 2;
                return converter;
            }
        }

        /// <summary>
        /// Creator class for Divide ValueConverter
        /// </summary>
        public class DivideCreator : ValueConverterCreator
        {
            public ValueConverter Create(ConvertRule rule, string type)
            {
                var converter = ScriptableObject.CreateInstance(type) as Divide;
                converter.Value = 2;
                return converter;
            }
        }

        /// <summary>
        /// Creator class for SetToZero ValueConverter
        /// </summary>
        public class SetToZeroCreator : ValueConverterCreator
        {
            public ValueConverter Create(ConvertRule rule, string type)
            {
                var converter = ScriptableObject.CreateInstance(type) as SetToZero;
                return converter;
            }
        }
    }
}


