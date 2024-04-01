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
    /// Class which implements the way actions are scheduled/played
    /// </summary>
    public class TurnPlayer : ITurnPlayer
    {
        public class Data
        {
            public BaseAction Action;
            public Action OnCompleted;
        }

        DateTime TimeStart;
        BattleEntity Battle;
        Coroutine TurnStartCoroutine;
        Coroutine TurnFinishCoroutine;
        List<Data> Scheduled = new List<Data>();
        List<Data> InProgress = new List<Data>();

        bool IsPlaying { get { return InProgress.Count > 0 || Scheduled.Count > 0; } }

        public float TimePassed
        {
            get
            {
                var delta = DateTime.UtcNow.Ticks - TimeStart.Ticks;
                return (float)(new TimeSpan(delta).TotalSeconds);
            }
        }

        public TurnPlayer(BattleEntity battle)
        {
            Battle = battle;
        }

        void ITurnPlayer.Start()
        {
            TurnStartInternal();
        }

        /// <summary>
        /// Do main loop actions at turn start
        /// </summary>
        void ITurnPlayer.TurnStart()
        {
            TurnStartInternal();
        }

        void ITurnPlayer.Play(BaseAction action, Action onCompleted)
        {
            Scheduled.Add(new Data { Action = action, OnCompleted = onCompleted });
            PlayTry();
        }

        /// <summary>
        /// Do main loop actions at turn finish
        /// </summary>
        void ITurnPlayer.TurnFinish()
        {
            if (Battle.State != BattleState.Started)
            {
                Log.E($"Can't start turn. Battle state: {Battle.State}");
                return;
            }

            if (Battle.TurnState != TurnState.Started)
            {
                Log.E("Can't finish turn. Turn didn't start");
                return;
            }

            if (IsPlaying)
            {
                Log.E("Can't finish turn. Turn is playing");
                return;
            }

            TurnFinishCoroutine = CoroutineHelper.Launch(Battle.TurnFinishing(), TurnFinishActions);
        }

        void ITurnPlayer.Destroy()
        {

        }

        void TurnStartInternal()
        {
            if (Battle.State != BattleState.Started)
            {
                Log.E($"Can't start turn. Battle state: {Battle.State}");
                return;
            }

            if (Battle.TurnState != TurnState.Finished && Battle.TurnState != TurnState.None)
            {
                Log.E("Can't start turn. Turn didn't finish");
                return;
            }

            if (Battle.UnitsTimeline.Count == 0)
            {
                Log.E("Can't start turn. Empty unit timeline sequence");
                return;
            }

            TurnStartCoroutine = CoroutineHelper.Launch(Battle.TurnStarting(), TurnStartActions);
        }

        /// <summary>
        /// Finish preparation actions at turn start
        /// </summary>
        void TurnStartActions()
        {
            TurnStartCoroutine = null;
            TimeStart = DateTime.UtcNow;
            Battle.TurnState = TurnState.Started;
            Battle.RaiseOnTurnStarted();
        }

        void PlayTry()
        {
            if (Scheduled.Count == 0)
            {
                return;
            }

            if (InProgress.Count > 0)
            {
                Log.I("Try play. Already in progress");
                return;
            }

            var next = Scheduled[0];
            Scheduled.RemoveAt(0);
            if (next.Action == null)
            {
                Log.E("Action won't play. Action Null");
                next.OnCompleted.SafeInvoke();
                PlayTry();
            }
            else if (Battle.Level.Actions.Validate(next.Action, Battle))
            {
                InProgress.Add(next);
                next.Action.Do(() =>
                {
                    InProgress.Remove(next);
                    Battle.TurnActions.Add(next.Action);
                    next.OnCompleted.SafeInvoke();
                    PlayTry();
                },
                Battle);
            }
            else
            {
                Log.E($"Action won't play. Action is invalid. {next.Action}");
                next.OnCompleted.SafeInvoke();
                PlayTry();
            }
        }

        /// <summary>
        /// Finish preparation actions at turn finish
        /// </summary>
        void TurnFinishActions()
        {
            TurnFinishCoroutine = null;
            if (S.Battle.AutoSave)
            {
                SaveController.SaveGame(Battle.Game, "1", TurnOnFinished);
            }
            else
            {
                TurnOnFinished();
            }
        }

        void TurnOnFinished()
        {
            if (Battle.State == BattleState.Finished)
            {
                Battle.RaiseOnBattleFinished();
            }
            else
            {
                TurnStartInternal();
            }
        }
    }
}
