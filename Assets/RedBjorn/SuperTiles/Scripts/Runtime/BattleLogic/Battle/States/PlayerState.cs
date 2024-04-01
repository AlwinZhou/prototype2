using System.Linq;

namespace RedBjorn.SuperTiles.Battle.States
{
    /// <summary>
    /// BattleView state which is a initial state for Player loop
    /// </summary>
    public class PlayerState : State
    {
        protected override void Enter()
        {
            //Update UI
            Controller.TeamPanelUpdate();
            Controller.ItemsUI.UpdateItems(Controller, (item) => ChangeState(new UnitItemState(item)));
        }

        public override void Update()
        {
            if (InputController.GetGameHotkeyUp(S.Input.Cancel))
            {
                ChangeState(new UnitSelection());
                return;
            }

            if (Controller.TryTurnFinish())
            {
                return;
            }

            //Check valid unit conditions
            if (Unit == null || Unit.IsDead)
            {
                Unit = Player.Squad.FirstOrDefault(u => !u.IsDead);
                if (Unit == null)
                {
                    Log.W($"Coundn't find any player unit alive. Go to {nameof(IdleState)}");
                    ChangeState(new IdleState());
                    return;
                }
            }

            //Check if can do MoveAction
            if (Unit.IsActive && Battle.Level.Actions.CanMove(Unit, Battle))
            {
                ChangeState(new UnitMoveState());
                return;
            }
        }

        public override void Exit()
        {

        }

        public override bool IsSaveable()
        {
            return Game.Loader != null;
        }
    }
}