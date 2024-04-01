using System.Collections.Generic;
using System.Linq;

namespace RedBjorn.SuperTiles.BattleFinish
{
    public class OneAllyTeamLeft : BattleFinishHandler
    {
        public class Ally
        {
            public List<SquadControllerEntity> Controllers = new List<SquadControllerEntity>();
            public List<UnitEntity> Units = new List<UnitEntity>();
        }

        public override void Handle(BattleEntity battle)
        {
            var players = new List<SquadControllerEntity>(battle.Players);
            var info = new List<Ally>();
            while(players.Count > 0)
            {
                var player = players[0];
                var allies = battle.Level.AllyPlayers(player, battle).ToList();
                var allyInfo = new Ally();
                foreach (var ally in allies)
                {
                    allyInfo.Controllers.Add(ally);
                    players.Remove(ally);
                    foreach(var unit in ally.Squad)
                    {
                        allyInfo.Units.Add(unit);
                    }
                }
                info.Add(allyInfo);
            }
            var aliveTeams = info.Where(team => team.Units.Any(u => !u.IsDead));
            if(aliveTeams.Count() <= 1)
            {
                battle.State = BattleState.Finished;
                battle.Winners.Clear();
                foreach (var aliveTeam in aliveTeams)
                {
                    foreach(var controller in aliveTeam.Controllers)
                    {
                        battle.Winners.Add(controller);
                    }
                }
            }
        }
    }
}