using RedBjorn.ProtoTiles;
using RedBjorn.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Unit state
    /// </summary>
    [Serializable]
    public class UnitEntity : UI.ITooltip, IMortal
    {
        [Serializable]
        public class UnitStatDictionary : SerializableDictionary<UnitStatTag, StatEntity> { } // Hack to serialize dictionary

        public int Id;
        public int SleepTurnDuration;
        public bool Incorporeal;

        public UnitData Data;
        public Vector3 SpawnPosition;
        public UnitHealthEntity Health;
        public UnitMoveEntity Mover;

        [NonSerialized]
        public UnitView View;
        public UnitStatDictionary Stats;
        [SerializeReference]
        public List<ItemEntity> Items = new List<ItemEntity>();
        [SerializeReference]
        public List<EffectEntity> Effects = new List<EffectEntity>();

        public StatEntity this[UnitStatTag stat] => Stats.TryGetOrDefault(stat);
        public StatEntity Max => this[S.Battle.Tags.Unit.Health];
        public MapEntity Map => Game.Battle.Map;
        public bool IsActive => SleepTurnDuration <= 0;

        [SerializeField]
        Vector3Int CachedPosition;
        public Vector3Int TilePosition
        {
            get
            {
                if (View != null)
                {
                    return View.Position;
                }
                return CachedPosition;
            }
            set
            {
                CachedPosition = value;
            }
        }

        [SerializeField]
        Quaternion CachedRotation;
        public Quaternion Rotation
        {
            get
            {
                if (View != null)
                {
                    return View.Rotation;
                }
                return CachedRotation;
            }
            set
            {
                CachedRotation = value;
            }
        }

        public Vector3 WorldPosition
        {
            get
            {
                if (View != null)
                {
                    return View.WorldPosition;
                }
                return Vector3.zero;
            }
            set
            {
                if (!IsDeadInternal)
                {
                    var tile = Map.Tile(value);
                    if (tile != null)
                    {
                        TilePosition = tile.Position;
                    }
                    if (View != null)
                    {
                        View.WorldPosition = value;
                    }
                }
            }
        }

        [SerializeField]
        bool IsDeadInternal;
        public bool IsDead => IsDeadInternal;

        public GameEntity Game { get; private set; }

        public event Action<EffectEntity> OnEffectAdded;
        public event Action<EffectEntity> OnEffectRemoved;
        public event Action<bool> OnDeadStateChanged;

        public UnitEntity(int id, Vector3 position, Quaternion rotation, UnitData data, GameEntity game)
            : this(id, position, rotation, data, false, game)
        {

        }

        public UnitEntity(int id, Vector3 position, Quaternion rotation, UnitData data, bool incorporeal, GameEntity game)
        {
            Id = id;
            Data = data;
            Incorporeal = incorporeal;
            Game = game;
            SpawnPosition = position;
            Stats = new UnitStatDictionary();
            foreach (var s in Data.Stats)
            {
                Stats[s.Stat] = new StatEntity(s.Value);
            }
            var tile = Game.Battle.Map.Tile(position);
            if (tile != null)
            {
                CachedPosition = tile.Position;
            }
            CachedRotation = rotation;
            Health = new UnitHealthEntity(this);
            Mover = new UnitMoveEntity(this);

            Data.DefaultItems.ForEach(item => AddItem(item));
            View = Data.CreateView(position, rotation);
            View.Init(game, this);
            Data.GetHealthAction().Handle(this, Game.Battle);
            Mover.CreateView();
        }

        public override string ToString()
        {
            if (Data == null)
            {
                return string.Format("Unit-{0}", Id.ToString());
            }
            return string.Format("{0}-{1}", Data.name, Id.ToString());
        }

        public void Load(GameEntity game)
        {
            Game = game;
            var position = game.Battle.Map.WorldPosition(CachedPosition);
            View = Data.CreateView(position, CachedRotation);
            View.Init(game, this);
            Health.Load(this);
            Data.GetHealthAction().Handle(this, Game.Battle);
            Mover.Load(this);
            Mover.CreateView();
            foreach (var item in Items)
            {
                item.Load();
            }
            foreach (var effect in Effects)
            {
                effect.Load(this);
            }
        }

        public void ViewShow()
        {
            if (View)
            {
                View.Show();
            }
        }

        public void ViewHide()
        {
            if (View)
            {
                View.Hide();
            }
        }

        public IEnumerator LookingAt(Vector3 position)
        {
            var rotating = true;
            Mover.Rotate(position, () => rotating = false);
            while (rotating)
            {
                yield return null;
            }
        }

        public void AddItem(ItemData item)
        {
            var itemEntity = new ItemEntity(item);
            Items.Add(itemEntity);
        }

        public void StatUpdate(UnitStatTag unitStat, StatEntity stat)
        {
            Stats[unitStat] = stat;
            Health.Update();
        }

        public IEnumerator AddEffect(EffectData effect, int duration, BattleEntity battle)
        {
            if (effect)
            {
                EffectEntity entity = null;
                if (battle.Level.EqualEffectInfluenceDuration)
                {
                    entity = Effects.FirstOrDefault(e => e.Data == effect);
                    if (entity != null)
                    {
                        entity.DurationAdd(duration);
                    }
                }
                if (entity == null)
                {
                    entity = new EffectEntity(effect, duration, this);
                    Effects.Add(entity);
                    yield return entity.OnAdded(battle);
                }
                OnEffectAdded.SafeInvoke(entity);
            }
        }

        public IEnumerator RemoveEffect(EffectData effect, BattleEntity battle)
        {
            if (effect)
            {
                var effectsRemove = Effects.Where(e => e.Data == effect).ToList();
                foreach (var ef in effectsRemove)
                {
                    yield return RemoveEffect(ef, battle);
                }
            }
        }

        public IEnumerator RemoveEffect(EffectEntity effect, BattleEntity battle)
        {
            if (effect != null)
            {
                Effects.Remove(effect);
                yield return effect.OnRemoved(battle);
                OnEffectRemoved.SafeInvoke(effect);
            }
        }

        public IEnumerator OnStartTurn(BattleEntity battle)
        {
            foreach (var effect in Effects)
            {
                yield return effect.Handle(battle);
            }

            var effectsRemove = Effects.Where(effect => effect.Duration <= 0).ToList();
            foreach (var effect in effectsRemove)
            {
                yield return RemoveEffect(effect, battle);
            }
        }

        public IEnumerator OnFinishTurn(BattleEntity battle)
        {
            if (SleepTurnDuration > 0)
            {
                SleepTurnDuration--;
            }
            Items.ForEach(item => item.OnFinishTurn());
            yield break;
        }

        public void Rebirth()
        {
            IsDeadInternal = false;
            Health.Restore();
            OnDeadStateChanged?.Invoke(IsDeadInternal);
        }

        public void OnMinValueReached()
        {
            IsDeadInternal = true;
            OnDeadStateChanged?.Invoke(IsDeadInternal);
        }

        string UI.ITooltip.Text()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"{ToString()}");
            sb.Append($"\nHealth: {Health.Text()}");
            var health = S.Battle.Tags.Unit.Health;
            var tags = S.Battle.Tags.Synonym;
            foreach (var k in Stats.Where(kv => kv.Key != health))
            {
                sb.Append($"\n{tags.Get(k.Key)}: {k.Value.Result}");
            }

            if (Data && Data.Tags.Count > 0)
            {
                sb.Append("\n\nTags");
                foreach (var tag in Data.Tags)
                {
                    if (tag)
                    {
                        sb.Append($"\n{tag.name}");
                    }
                }
            }

            sb.Append($"\n\nEffects:");
            if (Effects.Count == 0)
            {
                sb.AppendLine("\nNone");
            }
            else
            {
                foreach (var effect in Effects)
                {
                    sb.Append($"\n{effect}");
                }
            }
            return sb.ToString();
        }
    }
}