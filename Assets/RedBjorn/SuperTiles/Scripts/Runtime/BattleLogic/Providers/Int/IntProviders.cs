using System.Linq;

namespace RedBjorn.SuperTiles.IntProviders
{
    public class Multiply : IIntProvider
    {
        public IIntProvider First;
        public IIntProvider Second;

        public int Get(BattleEntity battle)
        {
            var first = First != null ? First.Get(battle) : 0;
            var second = Second != null ? Second.Get(battle): 0;
            return first * second;
        }
    }

    public class Constant : IIntProvider
    {
        public int Value;

        public int Get(BattleEntity battle)
        {
            return Value;
        }
    }
}