namespace RedBjorn.SuperTiles
{
    public interface IDiedAction
    {
        void Handle(UnitEntity unit, UnitEntity killer, BattleEntity battle);
    }
}
