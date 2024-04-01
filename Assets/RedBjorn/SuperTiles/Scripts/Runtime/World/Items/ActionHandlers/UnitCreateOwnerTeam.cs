using RedBjorn.SuperTiles.Battle.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items.ActionHandlers
{
    public class UnitCreateOwnerTeam : ActionHandler
    {
        [Serializable]
        public class TeamMapping
        {
            public bool Incorporeal;
            public TeamTag Team;
            public UnitData Unit;
            public UnitAiData Ai;
        }

        [Serializable]
        public class Resources
        {
            public ResourceTag Resource;
            public ItemStatTag Stat;
        }

        public List<TeamMapping> TeamInfo;
        public List<Resources> ResourcesCost;
        public int SleepTurnDuration;

        public override IEnumerator DoHandle(ItemAction data, BattleEntity battle)
        {
            if (TeamInfo == null)
            {
                yield break;
            }
            var controller = battle.Players.FirstOrDefault(p => p.Squad.Contains(data.Unit));
            if (controller == null)
            {
                yield break;
            }
            if (ResourcesCost != null)
            {
                if (ResourcesCost.Any(r => controller[r.Resource] < data.Item[r.Stat]))
                {
                    yield break;
                }
                else
                {
                    foreach (var k in ResourcesCost)
                    {
                        controller.Resources[k.Resource] -= data.Item[k.Stat];
                    }
                }
            }

            UnitData unit = null;
            UnitAiData ai = null;
            var incorporeal = false;
            var info = TeamInfo.FirstOrDefault(t => t.Team == controller.Team);
            if (info != null)
            {
                incorporeal = info.Incorporeal;
                unit = info.Unit;
                ai = info.Ai;
            }
            var rotation = data.Unit != null ? data.Unit.Rotation : Quaternion.identity;
            var entity = battle.CreateUnit(unit, data.Position, rotation, incorporeal, battle.Game);
            var aiEntity = battle.CreateAI(entity, ai);
            battle.TakeUnitControl(entity, controller);
            battle.TurnUnits.Add(entity);
            entity.SleepTurnDuration = SleepTurnDuration;
        }
    }
}
