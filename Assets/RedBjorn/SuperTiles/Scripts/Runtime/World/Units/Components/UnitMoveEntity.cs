using RedBjorn.Utils;
using System;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// State of move unit logic
    /// </summary>
    [Serializable]
    public class UnitMoveEntity
    {
        [NonSerialized]
        UnitEntity Unit;
        UnitMoveView View;
        
        public event Action OnMoveStarted;
        public event Action OnMoveFinished;

        public StatEntity MoveRange => Unit[S.Battle.Tags.Unit.MoveRange];

        public UnitMoveEntity(UnitEntity unit)
        {
            Unit = unit;
        }

        public void Load(UnitEntity unit)
        {
            Unit = unit;
        }

        public void CreateView()
        {
            View = Unit.View.gameObject.AddComponent<UnitMoveView>();
            View.Init(Unit, S.Battle.Tags.Unit.Speed, S.Battle.Tags.Unit.RotationSpeed);
        }

        public void Move(Vector3 point, Action onCompleted)
        {
            if (!Unit.IsDead)
            {
                Action onMoveCompleted = () =>
                {
                    OnMoveFinished.SafeInvoke();
                    onCompleted.SafeInvoke();
                };
                OnMoveStarted.SafeInvoke();
                View.Move(point, MoveRange, onMoveCompleted);
            }
        }

        public void Rotate(Vector3 point, Action onCompleted)
        {
            if (!Unit.IsDead)
            {
                View.Rotate(point, onCompleted);
            }
        }

    }
}
