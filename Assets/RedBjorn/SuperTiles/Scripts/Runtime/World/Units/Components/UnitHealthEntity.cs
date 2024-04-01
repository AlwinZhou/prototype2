using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Health state of unit
    /// </summary>
    [Serializable]
    public class UnitHealthEntity
    {
        [SerializeField]
        float Current;
        [SerializeField]
        float Ratio;
        [SerializeField] 
        List<float> Scheduled;

        IMortal HolderEntity;

        public StatEntity Max => HolderEntity.Max;
        public float ValueCurrent => Current;

        public event Action<HealthChangeContext> OnHeathChanged;

        public UnitHealthEntity(IMortal holderEntity)
        {
            Scheduled = new List<float>();
            HolderEntity = holderEntity;
            Current = HolderEntity.Max;
            Ratio = 1f;
        }

        public void Load(IMortal holderEntity)
        {
            HolderEntity = holderEntity;
        }

        public void Update()
        {
            if (HolderEntity.IsDead)
            {
                return;
            }
            if (Current <= 0f)
            {
                HolderEntity.OnMinValueReached();
                return;
            }

            var previous = Current;
            if (Current > HolderEntity.Max)
            {
                Current = HolderEntity.Max * Ratio;
            }
            Ratio = Current / HolderEntity.Max;
            if (Mathf.Abs(Current - previous) < 0.01f)
            {
                OnHeathChanged.SafeInvoke(new HealthChangeContext() { Current = Current, Previous = previous, MaxHealth = HolderEntity.Max });
            }
        }

        public void Change(float delta)
        {
            if (!HolderEntity.IsDead)
            {
                ChangeInternal(delta);
            }
        }

        public void Restore()
        {
            ChangeInternal(Max - Current);
        }

        void ChangeInternal(float delta)
        {
            Scheduled.Add(delta);
            PlayChange();
        }

        void PlayChange()
        {
            if (Scheduled.Count == 0)
            {
                Log.I("Health. Nothing to play");
                return;
            }
            var delta = Scheduled[0];
            Scheduled.RemoveAt(0);

            var previous = Current;
            Current += delta;
            if (Current <= 0f)
            {
                HolderEntity.OnMinValueReached();
            }
            if (Current > HolderEntity.Max)
            {
                Current = HolderEntity.Max;
            }
            Ratio = Current / HolderEntity.Max;
            OnHeathChanged.SafeInvoke(new HealthChangeContext() { Current = Current, Previous = previous, MaxHealth = HolderEntity.Max });
            PlayChange();
        }

        public string Text()
        {
            return $"{Current}/{HolderEntity.Max.Result}";
        }
    }
}