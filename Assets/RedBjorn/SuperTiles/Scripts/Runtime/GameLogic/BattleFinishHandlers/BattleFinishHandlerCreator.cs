using UnityEngine;

namespace RedBjorn.SuperTiles
{
    public interface BattleFinishHandlerCreator
    {
        BattleFinishHandler Create(string type);
    }

    namespace BattleFinish
    {
        public class DefaultCreator : BattleFinishHandlerCreator
        {
            public BattleFinishHandler Create(string type)
            {
                return ScriptableObject.CreateInstance(type) as BattleFinishHandler;
            }
        }

        public class OneSquadLeftCreator : BattleFinishHandlerCreator
        {
            public BattleFinishHandler Create(string type)
            {
                return ScriptableObject.CreateInstance(type) as OneSquadLeft;
            }
        }

        public class OneAllyTeamLeftCreator : BattleFinishHandlerCreator
        {
            public BattleFinishHandler Create(string type)
            {
                return ScriptableObject.CreateInstance(type) as OneAllyTeamLeft;
            }
        }
    }
}