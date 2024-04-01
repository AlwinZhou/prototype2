using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.UI
{
    public class ItemsPanelUI : MonoBehaviour
    {
        public ItemUI ItemRef;
        List<ItemUI> ItemsUI = new List<ItemUI>();

        void Awake()
        {
            ItemRef.Deactivate();
            ItemRef.gameObject.SetActive(false);
        }

        public void UpdateItems(BattleView controller, Action<ItemEntity> onStartActivation)
        {
            foreach (var ui in ItemsUI)
            {
                Spawner.Despawn(ui.gameObject);
            }
            ItemsUI.Clear();

            if (controller.Unit != null)
            {
                var canDoItemActions = !controller.Unit.IsDead && controller.Unit.IsActive && controller.Battle.Level.Actions.CanItem(controller.Unit, controller.Battle);
                foreach (var item in controller.Unit.Items)
                {
                    var ui = Spawner.Spawn(ItemRef, ItemRef.transform.parent);
                    var available = canDoItemActions && !controller.Battle.TurnItems.Contains(item);
                    available &= item.CanUse(controller.Unit);
                    ui.Init(item, available, onStartActivation);
                    ui.gameObject.SetActive(true);
                    ItemsUI.Add(ui);
                }
            }
        }

        public void Select(ItemEntity item)
        {
            foreach (var ui in ItemsUI)
            {
                if (ui.Item == item)
                {
                    ui.SelectState();
                }
                else
                {
                    ui.UninteractableState();
                }
            }
        }
    }
}
