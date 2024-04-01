using RedBjorn.ProtoTiles;
using RedBjorn.SuperTiles.Battle;
using RedBjorn.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Battle state
    /// </summary>
    [Serializable]
    public class BattleEntity
    {
        /// <summary>
        /// Current battle state
        /// </summary>
        public BattleState State;

        /// <summary>
        /// Current turn
        /// </summary>
        public int Turn;

        /// <summary>
        /// Current turn state
        /// </summary>
        public TurnState TurnState;

        public float TurnDuration;
        /// <summary>
        /// Selected player
        /// </summary>
        [SerializeReference]
        public SquadControllerEntity Player;

        /// <summary>
        /// All squad controllers at current battle 
        /// </summary>
        [SerializeReference]
        public List<SquadControllerEntity> Players = new List<SquadControllerEntity>();

        [SerializeReference]
        public List<SquadControllerEntity> Winners = new List<SquadControllerEntity>();

        /// <summary>
        /// Id to mark next registered unit
        /// </summary>
        public int UnitId;

        /// <summary>
        /// Selected unit
        /// </summary>
        [SerializeReference]
        public UnitEntity Unit;

        /// <summary>
        /// All alive units at current battle
        /// </summary>
        [SerializeReference]
        public List<UnitEntity> UnitsAlive = new List<UnitEntity>();

        /// <summary>
        /// All dead units at current battle
        /// </summary>
        [SerializeReference]
        public List<UnitEntity> UnitsDead = new List<UnitEntity>();

        /// <summary>
        /// All ai which control alive units
        /// </summary>
        [SerializeReference]
        public List<UnitAiEntity> UnitAis = new List<UnitAiEntity>();

        /// <summary>
        /// Order of units turns
        /// </summary>
        [SerializeReference]
        public List<UnitEntity> UnitsTimeline = new List<UnitEntity>();

        /// <summary>
        /// Units which can make actions during current turn
        /// </summary>
        [SerializeReference]
        public List<UnitEntity> TurnUnits = new List<UnitEntity>();

        /// <summary>
        /// Items which were used during current turn
        /// </summary>
        [SerializeReference]
        public List<ItemEntity> TurnItems = new List<ItemEntity>();

        /// <summary>
        /// Actions which are already completed during current turn
        /// </summary>
        [SerializeReference]
        public List<BaseAction> TurnActions = new List<BaseAction>();

        /// <summary>
        /// Selected level
        /// </summary>
        [NonSerialized]
        public LevelData Level;

        /// <summary>
        /// Current map state
        /// </summary>
        [NonSerialized]
        public MapEntity Map;

        /// <summary>
        /// Short access to game state
        /// </summary>
        [NonSerialized]
        public GameEntity Game;

        /// <summary>
        /// Player that handles IAction
        /// </summary>
        [NonSerialized]
        public ITurnPlayer TurnPlayer;

        public System.Random Randomizer { get; private set; }

        public event Action OnTurnStarted;
        public event Action OnTurnFinishStarted;
        public event Action OnBattleFinished;

        public BattleEntity()
        {
            Turn = 0;
            TurnDuration = -1f;
            UnitId = 1;
            State = BattleState.None;
            TurnState = TurnState.None;
        }

        public void Destroy()
        {
            TurnPlayer.Destroy();
            Log.I("Battle destroyed");
        }

        public void RandomCreate(int randomSeed)
        {
            Randomizer = new System.Random(randomSeed);
        }

        public UnitEntity CreateUnit(UnitData data, Vector3 position, Quaternion rotation, bool incorporeal, GameEntity game)
        {
            var unit = new UnitEntity(UnitId, position, rotation, data, incorporeal, game);
            RegisterUnit(unit);
            Map.RegisterUnit(unit);
            return unit;
        }

        public UnitAiEntity CreateAI(UnitEntity unit, UnitAiData aiData)
        {
            var ai = new UnitAiEntity() { Unit = unit, Data = aiData };
            RegisterUnitAi(ai);
            return ai;
        }

        public void RegisterUnit(UnitEntity unit)
        {
            UnitsAlive.Add(unit);
            UnitId++;
        }

        public void RegisterUnitAi(UnitAiEntity ai)
        {
            UnitAis.Add(ai);
        }

        public void TakeUnitControl(UnitEntity unit, SquadControllerEntity capturedBy)
        {
            if (capturedBy != null)
            {
                foreach (var previousOwner in Players.Where(p => p.Squad.Contains(unit)))
                {
                    previousOwner.RemoveUnit(unit);
                }
                capturedBy.AddUnit(unit);
                Level.Turn.TurnSequenceInit(this);
                unit.Data.GetUnitChangeControllerAction()?.Handle(unit, capturedBy, this);
            }
        }

        /// <summary>
        /// Do preparation actions for battle start
        /// </summary>
        public void BattleStart()
        {
            if (State != BattleState.None)
            {
                Log.E("Battle already started");
                return;
            }

            State = BattleState.Started;
            Level.Turn.TurnSequenceInit(this);
            Level.GetBattleStartAction()?.Handle(this);
            TurnPlayer.Start();
        }

        /// <summary>
        /// Do preparations at turn start
        /// </summary>
        public IEnumerator TurnStarting()
        {
            TurnState = TurnState.Starting;
            Turn++;
            Log.I($"Turn: {Turn.ToString()} {TurnState}");
            Unit = UnitsTimeline[0]; //Select default unit
            Player = Players.FirstOrDefault(p => p.Squad.Contains(Unit)); //Select new player
            Level.GetTurnStartAction()?.Handle(this);
            Level.Turn.TurnSequenceStart(this);
            foreach (var unit in TurnUnits)
            {
                yield return unit.OnStartTurn(this);
                foreach (var ai in UnitAis.Where(a => a.Unit == unit))
                {
                    ai.OnStartTurn();
                }
            }
        }

        public void RaiseOnTurnStarted()
        {
            Log.I($"Turn: {Turn.ToString()} {TurnState}");
            OnTurnStarted.SafeInvoke();
        }

        /// <summary>
        /// Do preparation actions at turn finish
        /// </summary>
        public IEnumerator TurnFinishing()
        {
            OnTurnFinishStarted.SafeInvoke();
            TurnState = TurnState.Finishing;
            Level.GetTurnFinishAction()?.Handle(this);
            //Handle end turn action for Units and Ai
            foreach (var unit in TurnUnits)
            {
                yield return unit.OnFinishTurn(this);
                foreach (var ai in UnitAis.Where(a => a.Unit == unit))
                {
                    ai.OnFinishTurn();
                }
            }

            TurnActions.Clear();
            TurnItems.Clear();

            //Update Alive and Dead unit list
            for (int i = UnitsAlive.Count - 1; i >= 0; i--)
            {
                var unit = UnitsAlive[i];
                if (unit.IsDead)
                {
                    UnitsTimeline.Remove(unit);
                    UnitsAlive.RemoveAt(i);
                    UnitsDead.Add(unit);
                }
            }

            //Update map
            Map.Clear();
            foreach (var u in UnitsAlive)
            {
                Map.RegisterUnit(u);
            }

            //Check battle finish
            Level.BattleFinish.Handle(this);
            //Calculate units for next turn
            Level.Turn.TurnSequenceFinish(this);
            TurnState = TurnState.Finished;
            Log.I($"Turn: {Turn.ToString()} {TurnState}");
        }

        public void RaiseOnBattleFinished()
        {
            OnBattleFinished.SafeInvoke();
        }
    }
}