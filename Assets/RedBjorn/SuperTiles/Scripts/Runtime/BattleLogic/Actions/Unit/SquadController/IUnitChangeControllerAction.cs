namespace RedBjorn.SuperTiles
{
    public interface IUnitChangeControllerAction
    {
        void Handle(UnitEntity unit, SquadControllerEntity newController, BattleEntity battle);
    }
}