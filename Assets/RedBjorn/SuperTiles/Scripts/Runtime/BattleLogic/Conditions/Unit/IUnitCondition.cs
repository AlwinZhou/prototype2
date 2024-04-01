namespace RedBjorn.SuperTiles
{
    public interface IUnitCondition
    {
        bool IsMet(UnitEntity unit, BattleEntity battle);
    }
}
