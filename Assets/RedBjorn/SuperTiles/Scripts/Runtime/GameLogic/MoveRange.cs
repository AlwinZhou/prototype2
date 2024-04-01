using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.Resolvers
{
    /// <summary>
    /// TurnResolver which trasit turn from unit with highest MoveRange value to lowest
    /// </summary>
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Level.TurnResolver.MoveRange)]
    public class MoveRange : TurnResolver
    {
        public override bool CanChangeUnit => false;

        public override void TurnSequenceInit(BattleEntity battle)
        {
            var stat = S.Battle.Tags.Unit.MoveRange;
            battle.UnitsTimeline = battle.UnitsAlive.Select(u => new { unit = u, player = battle.Players.FirstOrDefault(p => p.Squad.Contains(u)) })
                                                    .Where(u => u.player != null)
                                                    .OrderByDescending(d => d.unit.Stats.TryGetOrDefault(stat).Result)
                                                    .ThenBy(d => d.player != null ? d.player.Team.name : $"z{d.unit.Id}")
                                                    .ThenBy(d => d.unit.Id)
                                                    .Select(d => d.unit)
                                                    .ToList();
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Turn Unit sequence:");
            foreach (var unit in battle.UnitsTimeline)
            {
                sb.Append($"{unit}: ");
                sb.Append($"Move range = {unit.Stats.TryGetOrDefault(stat).Result}. ");
                sb.AppendLine($"Player = {battle.Players.FirstOrDefault(p => p.Squad.Contains(unit))}");
            }
            Log.I(sb.ToString());
        }

        public override void TurnSequenceStart(BattleEntity battle)
        {
            battle.TurnUnits.Clear();
            battle.TurnUnits.Add(battle.Unit);
        }

        public override void TurnSequenceFinish(BattleEntity battle)
        {
            if (battle.UnitsTimeline.Count > 0)
            {
                var unit = battle.UnitsTimeline[0];
                battle.UnitsTimeline.RemoveAt(0);
                battle.UnitsTimeline.Add(unit);
            }
        }
    }
}
