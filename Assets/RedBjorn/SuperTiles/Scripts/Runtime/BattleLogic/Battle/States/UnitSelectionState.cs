namespace RedBjorn.SuperTiles.Battle.States
{
    public class UnitSelection : State
    {
        protected override void Enter()
        {
            Unit = null;
            Controller.TeamPanelUpdate();
            Controller.ItemsUI.UpdateItems(Controller, null);
        }

        public override void Update()
        {
            if (Controller.TryTurnFinish())
            {
                return;
            }

            var plane = Map.Settings.Plane();
            if (InputController.GetOnWorldUp(plane, true))
            {
                var current = InputController.OverGameobject(plane);
                if (current && current.TryGetComponent(out InteractableGameobjectLink link) && link.Rerefence)
                {
                    current = link.Rerefence;
                }
                if (current && current.TryGetComponent(out UnitView view))
                {
                    if (Controller.TryUnitSelect(view.Unit))
                    {
                        return;
                    }
                }

                var target = InputController.GroundPosition(plane);
                if (Controller.TryUnitSelect(target))
                {
                    return;
                }
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
