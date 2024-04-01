using RedBjorn.SuperTiles.Battle;
using RedBjorn.SuperTiles.Squad;
using System;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// State of Ai controlling single unit
    /// </summary>
    [Serializable]
    public class UnitAiEntity
    {
        public int TurnActionCount;
        public UnitAiData Data;
        [SerializeReference]
        public UnitEntity Unit;

        public bool TryNextAction(AiEntity player, BattleEntity battle, out BaseAction action)
        {
            if (Data == null)
            {
                action = null;
                return false;
            }
            return Data.TryNextAction(player, this, battle, out action);
        }

        public void OnStartTurn()
        {
            TurnActionCount = 0;
        }

        public void OnFinishTurn()
        {
            TurnActionCount = 0;
        }

        public void UpdateCount()
        {
            TurnActionCount++;
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]", Data ? Data.name : "None", Unit);
        }
    }
}