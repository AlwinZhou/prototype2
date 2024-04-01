using RedBjorn.SuperTiles.Battle.Actions;
using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Item state
    /// </summary>
    [Serializable]
    public class ItemEntity : UI.ITooltip
    {
        [Serializable]
        public class ItemStatDictionary : SerializableDictionary<ItemStatTag, StatEntity> { } // Hack to serialize dictionary

        public bool Used;
        public int CurrentCooldown;
        public int CurrentStackCount;
        public ItemData Data;
        public ItemStatDictionary Stats;

        public StatEntity this[ItemStatTag stat] => stat ? Stats.TryGetOrDefault(stat) : new StatEntity();

        ItemEntity() { }

        public ItemEntity(ItemData data)
        {
            Data = data;
            CurrentCooldown = 0;
            CurrentStackCount = Data.MaxStackCount;
            Stats = new ItemStatDictionary();
            foreach (var s in Data.Stats)
            {
                Stats[s.Stat] = new StatEntity(s.Value);
            }
        }

        public override string ToString()
        {
            return Data == null ? null : Data.name;
        }

        public void Load()
        {

        }

        public IEnumerable<UnitEntity> PossibleTargets(Vector3 attackPosition, BattleEntity battle)
        {
            return Data.Selector.PossibleTargets(this, attackPosition, battle);
        }

        public bool CanUse(UnitEntity unit)
        {
            var can = !Used;
            can &= CurrentCooldown == 0;
            can &= (!Data.Stackable || CurrentStackCount > 0f);
            can &= Data.GetCondition().IsMet(this, unit);
            return can; 
        }

        public void Use(ItemAction action, BattleEntity battle, Action onCompleted)
        {
            if (CanUse(action.Unit))
            {
                Used = true;
                if (Data.Stackable)
                {
                    CurrentStackCount = Mathf.Max(0, CurrentStackCount - 1);
                }
                CurrentCooldown = this[S.Battle.Tags.Item.Cooldown];
                CoroutineHelper.Launch(Data.ActionHandler.Handle(action, battle), onCompleted);
            }
            else
            {
                string actionInfo = "Null";
                if (action != null)
                {
                    actionInfo = action.ToString();
                }
                Log.W($"Can't use item for action: {actionInfo}");
                onCompleted.SafeInvoke();
            }
        }

        public void OnFinishTurn()
        {
            HandleCooldown();
        }

        void HandleCooldown()
        {
            if (Used)
            {
                Used = false;
            }
            else
            {
                CurrentCooldown = Mathf.Max(0, --CurrentCooldown);
            }
        }

        string UI.ITooltip.Text()
        {
            var sb = new System.Text.StringBuilder();
            if (Data)
            {
                sb.AppendLine(Data.Caption);
                var description = Data.Description;
                if (!string.IsNullOrEmpty(description))
                {
                    foreach (var match in System.Text.RegularExpressions.Regex.Matches(Data.Description, "\\{.*\\}"))
                    {
                        var result = match.ToString();
                        if (result.Length > 1)
                        {
                            var pair = Stats.FirstOrDefault(s => s.Key.name == result.Substring(1, result.Length - 2));
                            description = description.Replace(result, pair.Value.Result.ToString());
                        }
                    }
                    sb.AppendLine($"\n{description}");
                }
            }
            else
            {
                sb.AppendLine("(Unknown)");
            }

            foreach (var pair in Stats)
            {
                sb.Append($"\n{S.Battle.Tags.Synonym.Get(pair.Key)}: {pair.Value.Result}");
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
            return sb.ToString();
        }
    }
}

