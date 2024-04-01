using System.Linq;

namespace RedBjorn.SuperTiles.FloatProviders
{
    public class Multiply : IFloatProvider
    {
        public IFloatProvider First;
        public IFloatProvider Second;

        public float Get(BattleEntity battle)
        {
            var first = First != null ? First.Get(battle) : 0f;
            var second = Second != null ? Second.Get(battle) : 0f;
            return first * second;
        }
    }

    public class Constant : IFloatProvider
    {
        public float Value;

        public float Get(BattleEntity battle)
        {
            return Value;
        }
    }

    public class UnitsCountContainsTag : IFloatProvider
    {
        public IUnitSelector Unit;
        public UnitTag Tag;

        public float Get(BattleEntity battle)
        {
            if (Unit == null)
            {
                return 0f;
            }
            return Unit.Select(battle).Count(s => s.Data.Tags.Contains(Tag));
        }
    }
}
