using System.Linq;

namespace RedBjorn.SuperTiles.BattleFinish
{
    public class OneSquadLeft : BattleFinishHandler
    {
        public override void Handle(BattleEntity battle)
        {
            var squadLeft = battle.Players.Where(p => p.Squad.Any(u => !u.IsDead));
            if (squadLeft.Count() <= 1)
            {
                battle.State = BattleState.Finished;
                Log.I($"Battle state: {battle.State}");
                battle.Winners.Clear();
                foreach (var squad in squadLeft)
                {
                    battle.Winners.Add(squad);
                }
            }
        }
    }
}