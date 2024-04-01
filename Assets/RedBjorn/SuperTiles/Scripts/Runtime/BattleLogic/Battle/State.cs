using RedBjorn.ProtoTiles;

namespace RedBjorn.SuperTiles.Battle
{
    /// <summary>
    /// Base class for BattleView state
    /// </summary>
    public abstract class State
    {
        protected BattleView Controller;
        protected BattleEntity Battle { get { return Controller.Battle; } }
        protected MapEntity Map { get { return Controller.Map; } }
        protected UnitEntity Unit { get { return Battle.Unit; } set { Battle.Unit = value; } }
        protected SquadControllerEntity Player { get { return Battle.Player; } }
        protected GameEntity Game { get { return Controller.Game; } }

        public void OnEntered(BattleView controller)
        {
            Controller = controller;
            Enter();
        }
        protected abstract void Enter();
        public abstract void Update();
        public abstract void Exit();

        public virtual void OnUnitChanged()
        {
            ChangeState(new States.PlayerState());
        }

        public virtual bool IsSaveable()
        {
            return false;
        }

        public virtual bool IsLoadable()
        {
            return Game != null && Game.Loader != null;
        }

        public virtual bool IsRestartable()
        {
            return Game != null && Game.Restartable;
        }

        public virtual void OnTurnStarted()
        {
            Battle.Player.OnMyTurnstarted();
        }

        public virtual void OnTurnFinishStarted()
        {
            Controller.Status = Controller.Statuses.TurnFinish;
            ChangeState(new States.SpectatorState());
        }

        public virtual void OnBattleFinish()
        {
            ChangeState(new States.BattleFinishState());
        }

        public void ChangeState(State newState)
        {
            Controller.ChangeState(newState);
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
