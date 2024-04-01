using System.Linq;

namespace RedBjorn.SuperTiles.Battle.States
{
    /// <summary>
    /// BattleView state when Player observe playing scheduled IAction
    /// </summary>
    public class SpectatorState : State
    {
        protected override void Enter()
        {
            UpdateUI();
        }

        public override void Update()
        {
            var plane = Map.Settings.Plane();
            if (InputController.GetOnWorldUp(plane, true))
            {
                var target = InputController.GroundPosition(plane);
                Controller.TryUnitSelect(target);
            }
        }

        public override void Exit() { }

        public override void OnTurnStarted()
        {
            var turnStart = S.Text.TurnStart;
            if (Controller.IsMyPlayer)
            {
                Controller.Status = Controller.Statuses.TurnMy;
                UI.TurnPlayerMyUI.Show(string.Format(turnStart.My, Battle.Player.Nickname),
                    () =>
                    {
                        ChangeState(new PlayerState());
                        Battle.Player.OnMyTurnstarted();
                    });
            }
            else
            {
                var turnLabel = string.Empty;
                if (Controller.Owners.Any(o => Battle.Level.IsAllies(o, Battle.Player)))
                {
                    turnLabel = string.Format(turnStart.Ally, Battle.Player.Nickname);
                }
                else if (Controller.Owners.Any(o => Battle.Level.IsEnemies(o, Battle.Player)))
                {
                    turnLabel = string.Format(turnStart.Enemy, Battle.Player.Nickname);
                }
                Controller.Status = string.Format(Controller.Statuses.TurnOther, Battle.Player.Nickname);
                UI.TurnPlayerOtherUI.Show(turnLabel,
                    () =>
                    {
                        Battle.Player.OnMyTurnstarted();
                    });
            }
        }

        public override void OnUnitChanged()
        {
            UpdateUI();
        }

        public override void OnTurnFinishStarted()
        {

        }

        void UpdateUI()
        {
            Controller.TeamPanelUpdate();
            Controller.ItemsUI.UpdateItems(Controller, null);
        }
    }
}
