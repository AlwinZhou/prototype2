using RedBjorn.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Effect state
    /// </summary>
    [Serializable]
    public class EffectEntity
    {
        [Serializable]
        public class EffectStatDictionary : SerializableDictionary<EffectStatTag, StatEntity> { } // Hack to serialize dictionary

        public int Duration;
        public EffectData Data;
        public EffectStatDictionary Stats;
        [NonSerialized]
        UnitEntity Owner;

        public SpriteRenderer UI { get; private set; }

        public StatEntity this[EffectStatTag stat] { get { return Stats.TryGetOrDefault(stat); } }

        public EffectEntity(EffectData effect, UnitEntity owner)
        {
            Data = effect;
            Stats = new EffectStatDictionary();
            foreach (var s in Data.Stats)
            {
                Stats[s.Stat] = new StatEntity(s.Value);
            }
            Duration = 1;
        }

        public EffectEntity(EffectData effect, int duration, UnitEntity owner)
        {
            Data = effect;
            Owner = owner;
            Stats = new EffectStatDictionary();
            foreach (var s in Data.Stats)
            {
                Stats[s.Stat] = new StatEntity(s.Value);
            }
            Duration = duration;
        }

        public void Load(UnitEntity owner)
        {
            Owner = owner;
        }

        public override string ToString()
        {
            return $"{Data.Caption}({Duration})";
        }

        public IEnumerator OnAdded(BattleEntity battle)
        {
            if (Data.FxAddShow)
            {
                var handler = Data.FxAddHandler ?? S.Battle.EffectFxAddDefault;
                yield return handler.Handle(this, Owner, battle);
            }
            if (Data.OnAdded)
            {
                yield return Data.OnAdded.Handle(this, Owner, battle);
            }
        }

        public IEnumerator Handle(BattleEntity battle)
        {
            if (Data.Handler)
            {
                yield return Data.Handler.Handle(this, Owner, battle);
            }
            Duration--;
        }

        public IEnumerator OnRemoved(BattleEntity battle)
        {
            if (Data.FxRemoveShow)
            {
                var handler = Data.FxRemoveHandler ?? S.Battle.EffectFxRemoveDefault;
                yield return handler.Handle(this, Owner, battle);
            }
            if (Data.OnRemoved)
            {
                yield return Data.OnRemoved.Handle(this, Owner, battle);
            }
        }

        public void DurationAdd(int delta)
        {
            Duration += delta;
        }
    }
}
