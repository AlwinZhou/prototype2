using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Interface for TurnResolver creation from string type
    /// </summary>
    public interface TurnResolverCreator
    {
        TurnResolver Create(string type);
    }

    namespace Resolvers
    {
        /// <summary>
        /// Default creator for TurnResolver with initial values
        /// </summary>
        public class DefaultCreator : TurnResolverCreator
        {
            public TurnResolver Create(string type)
            {
                return ScriptableObject.CreateInstance(type) as TurnResolver;
            }
        }

        /// <summary>
        /// Creator class for Squad TurnResolver
        /// </summary>
        public class SquadCreator : TurnResolverCreator
        {
            public TurnResolver Create(string type)
            {
                var selector = ScriptableObject.CreateInstance(type) as Squad;
                return selector;
            }
        }

        /// <summary>
        /// Creator class for MoveRange TurnResolver
        /// </summary>
        public class MoveRangeCreator : TurnResolverCreator
        {
            public TurnResolver Create(string type)
            {
                var selector = ScriptableObject.CreateInstance(type) as MoveRange;
                return selector;
            }
        }
    }
}

