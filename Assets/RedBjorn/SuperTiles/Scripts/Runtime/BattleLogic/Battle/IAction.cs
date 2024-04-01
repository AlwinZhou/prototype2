using System;

namespace RedBjorn.SuperTiles.Battle
{
    /// <summary>
    /// Base class for unit action
    /// </summary>
    [Serializable]
    public abstract partial class BaseAction
    {
        public abstract SquadControllerEntity Player { get; }
        public abstract UnitEntity Unit { get; }
        public abstract void Do(Action onCompleted, BattleEntity battle);
    }
}