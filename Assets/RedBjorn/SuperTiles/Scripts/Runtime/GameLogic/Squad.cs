using RedBjorn.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.Resolvers
{
    /// <summary>
    /// TurnResolver which allows all squad units complete actions before turn will be transit to the next squad
    /// </summary>
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Level.TurnResolver.Squad)]
    public class Squad : TurnResolver
    {
        public override bool CanChangeUnit => true;

        public override void TurnSequenceInit(BattleEntity battle)
        {
            battle.UnitsTimeline = battle.UnitsAlive.Select(u => new { unit = u, player = battle.Players.FirstOrDefault(p => p.Squad.Contains(u)) })
                                                    .Where(u => u.player != null)
                                                    .OrderBy(d => d.player != null ? d.player.Team.name : $"z{d.unit.Id}")
                                                    .ThenBy(d => d.unit.Id)
                                                    .Select(d => d.unit)
                                                    .ToList();
            Log.I($"Init round sequence:\n{battle.UnitsTimeline.ToColumn()}");
        }

        public override void TurnSequenceStart(BattleEntity battle)
        {
            battle.TurnUnits.Clear();
            foreach (var unit in battle.UnitsTimeline)
            {
                var player = battle.Players.FirstOrDefault(b => b.Squad.Contains(unit));
                if (player == null || player != battle.Player)
                {
                    break;
                }
                battle.TurnUnits.Add(unit);
            }
        }

        public override void TurnSequenceFinish(BattleEntity battle)
        {
            if (battle.State == BattleState.Finished)
            {
                return;
            }
            var index = battle.UnitsTimeline.FindIndex(u => battle.Players.FirstOrDefault(p => p.Squad.Contains(u)) != battle.Player);
            if (index < 0)
            {
                Log.E($"Can't calculate approporiate turn sequence.\n{battle.UnitsTimeline.ToColumn()}");
            }
            else
            {
                var temp = new List<UnitEntity>();
                for (int i = index; i < battle.UnitsTimeline.Count; i++)
                {
                    temp.Add(battle.UnitsTimeline[i]);
                }
                for (int i = 0; i < index; i++)
                {
                    temp.Add(battle.UnitsTimeline[i]);
                }
                battle.UnitsTimeline = temp;
            }
        }
    }
}
