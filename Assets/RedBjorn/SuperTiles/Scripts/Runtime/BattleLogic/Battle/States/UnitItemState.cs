using RedBjorn.SuperTiles.Battle.Actions;
using RedBjorn.SuperTiles.Items;

namespace RedBjorn.SuperTiles.Battle.States
{
    /// <summary>
    /// BattleView state which controls player ItemAction handling
    /// </summary>
    public class UnitItemState : State
    {
        ItemEntity Item;
        ITargetSelectorView ItemView;

        UnitItemState() { }

        public UnitItemState(ItemEntity item)
        {
            Item = item;
        }

        protected override void Enter()
        {
            ItemSelectorStart();
            Controller.ItemsUI.Select(Item);
        }

        public override void Update()
        {
            if (InputController.GetGameHotkeyUp(S.Input.Cancel))
            {
                if (ItemView != null)
                {
                    ChangeState(new PlayerState());
                    return;
                }
            }

            if (Controller.TryTurnFinish())
            {
                return;
            }
        }

        public override void Exit()
        {
            if (ItemView != null)
            {
                ItemView.Abort();
            }
        }

        public override bool IsSaveable()
        {
            return Game.Loader != null;
        }

        void ItemSelectorStart()
        {
            //Create view for Item selector
            var data = new ItemAction(Battle.Player, Controller.Unit, Item);
            ItemView = Item.Data.Selector.StartActivation(data, ItemSelectorFinish, Controller);
        }

        void ItemSelectorFinish(ItemAction action)
        {
            //Play created ItemAction
            ItemView = null;
            ChangeState(new SpectatorState());
            Battle.TurnPlayer.Play(action, () => ChangeState(new PlayerState()));
        }
    }
}