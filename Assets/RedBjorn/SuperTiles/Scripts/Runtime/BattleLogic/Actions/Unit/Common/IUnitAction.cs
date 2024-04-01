namespace RedBjorn.SuperTiles
{
    public interface IUnitAction
    {
        void Handle(UnitEntity unit, BattleEntity battle);
    }
}
