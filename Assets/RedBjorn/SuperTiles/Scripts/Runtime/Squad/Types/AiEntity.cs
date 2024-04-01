using RedBjorn.SuperTiles.Battle;
using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.Squad
{
    /// <summary>
    /// Ai player which controls several bots
    /// </summary>
    public class AiEntity : SquadControllerEntity
    {
        [SerializeReference]
        public List<UnitAiEntity> Bots = new List<UnitAiEntity>();

        const int IdOffset = 10000;

        BattleEntity Battle { get { return Game.Battle; } }

        public AiEntity(int id, TeamTag team, string nickname, GameEntity game) : base(IdOffset + id, team, game)
        {
            Nickname = nickname;
        }

        public void Load()
        {

        }

        public override string ToString()
        {
            return string.Format("Ai (id: {0})", Id);
        }

        public override void AddUnit(UnitEntity unit)
        {
            base.AddUnit(unit);
            foreach (var ai in unit.Game.Battle.UnitAis.Where(a => a.Unit == unit))
            {
                Bots.Add(ai);
            }
        }

        public override void RemoveUnit(UnitEntity unit)
        {
            base.RemoveUnit(unit);
            foreach (var ai in unit.Game.Battle.UnitAis.Where(a => a.Unit == unit))
            {
                Bots.Remove(ai);
            }
        }

        public override void OnMyTurnstarted()
        {
            Think(() => Battle.TurnPlayer.TurnFinish());
        }

        /// <summary>
        /// Input point to start Ai scheduling
        /// </summary>
        /// <param name="onCompleted"></param>
        public void Think(Action onCompleted)
        {
            var bots = new List<UnitAiEntity>();
            foreach (var unit in Battle.TurnUnits)
            {
                var ai = Bots.FirstOrDefault(b => b.Unit == unit);
                if (ai != null && !unit.IsDead)
                {
                    bots.Add(ai);
                }
            }
            bots.Shuffle(Battle.Randomizer);
            Think(bots, 0, onCompleted);
        }

        void Think(List<UnitAiEntity> bots, int depth, Action onCompleted)
        {
            //Check stop condition
            if (bots.Count == 0)
            {
                Log.I("All bot already played action. Finish thinking");
                onCompleted.SafeInvoke();
                return;
            }

            var bot = bots[0];

            //Check out of range action condition
            if (depth > bot.Unit.Items.Count + 1)
            {
                Log.I($"Try to act to many actions for single bot = {bot}");
                bots.RemoveAt(0);
                Think(bots, 0, onCompleted);
            }
            else
            {
                //Try to schedule new unit action
                BaseAction action;
                if (bot.TryNextAction(this, Battle, out action) && action != null)
                {
                    Log.I($"{bot} try play action: {action}");
                    Battle.TurnPlayer.Play(action, () => Think(bots, bot.TurnActionCount, onCompleted));
                }
                else
                {
                    //Cannot schedule new action for selected unit
                    //Go to nex unit
                    Log.I($"{bot}: No next action. Go to next bot");
                    bots.RemoveAt(0);
                    Think(bots, 0, onCompleted);
                }
            }
        }
    }
}
