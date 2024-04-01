using System;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Simple UnityEngine.Animator wrapper
    /// </summary>
    public class UnitAnimator : MonoBehaviour, IUnitInitialize
    {
        public Animator Animator;
        public bool SetSpeed;
        public bool SetWalk;

        string StateDeath = "Death";
        string StateHit = "Hit";
        string ParamWalk = "Walk";
        string ParamSpeed = "Speed";

        UnitEntity Unit;

        Settings.BattleSettings.TagsSettings.UnitSettings UnitTags => S.Battle.Tags.Unit;

        void OnDestroy()
        {
            if (Unit != null)
            {
                Unit.Health.OnHeathChanged -= OnHealthChanged;
                if (SetWalk)
                {
                    Unit.Mover.OnMoveStarted -= OnMoveStarted;
                    Unit.Mover.OnMoveFinished -= OnMoveFinished;
                }
            }
        }

        public void Init(UnitEntity unit)
        {
            if (Unit != null)
            {
                Unit.Health.OnHeathChanged -= OnHealthChanged;
                if (SetWalk)
                {
                    Unit.Mover.OnMoveStarted -= OnMoveStarted;
                    Unit.Mover.OnMoveFinished -= OnMoveFinished;
                }
            }
            Unit = unit;
            if (Unit != null)
            {
                Unit.Health.OnHeathChanged += OnHealthChanged;
                if (SetWalk)
                {
                    Unit.Mover.OnMoveStarted += OnMoveStarted;
                    Unit.Mover.OnMoveFinished += OnMoveFinished;
                }
                if (Unit.IsDead)
                {
                    if (!string.IsNullOrEmpty(StateDeath))
                    {
                        Animator.Play(StateDeath);
                    }
                    else
                    {
                        Log.W("Won't play death state. Null state");
                    }
                }

                if (SetSpeed)
                {
                    if (!string.IsNullOrEmpty(ParamSpeed))
                    {
                        Animator.SetFloat(ParamSpeed, Unit[UnitTags.Speed]);
                    }
                    else
                    {
                        Log.W("Won't set speed. Null parameter");
                    }
                }
            }
        }

        public void PlayState(string state)
        {
            if (!string.IsNullOrEmpty(state))
            {
                Animator.Play(state);
            }
            else
            {
                Log.W("Won't play state. Null state");
            }
        }

        void OnHealthChanged(HealthChangeContext context)
        {
            if (context.Current <= 0)
            {
                if (!string.IsNullOrEmpty(StateDeath))
                {
                    Animator.Play(StateDeath);
                }
                else
                {
                    Log.W("Won't play death state. Null state");
                }
            }
            else if (context.Previous > context.Current)
            {
                if (!string.IsNullOrEmpty(StateHit))
                {
                    Animator.Play(StateHit);
                }
                else
                {
                    Log.W("Won't play hit state. Null state");
                }
            }
        }

        void OnMoveStarted()
        {
            if (!string.IsNullOrEmpty(ParamWalk))
            {
                Animator.SetBool(ParamWalk, true);
            }
            else
            {
                Log.W("Won't play walk. Null parameter");
            }
        }

        void OnMoveFinished()
        {
            if (!string.IsNullOrEmpty(ParamWalk))
            {
                Animator.SetBool(ParamWalk, false);
            }
            else
            {
                Log.W("Won't play walk. Null parameter");
            }
        }
    }
}
