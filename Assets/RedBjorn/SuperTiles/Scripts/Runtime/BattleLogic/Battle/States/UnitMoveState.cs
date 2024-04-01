using RedBjorn.ProtoTiles;
using RedBjorn.SuperTiles.Battle.Actions;
using RedBjorn.SuperTiles.Settings;
using RedBjorn.SuperTiles.Utils;
using RedBjorn.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles.Battle.States
{
    /// <summary>
    /// BattleView state which controls player MoveAction handling
    /// </summary>
    public class UnitMoveState : State
    {
        HashSet<TileEntity> Walkable = new HashSet<TileEntity>();
        AreaOutline AreaOutline;
        PathDrawer Path;

        const float AreaOffset = 0.01f;

        PrefabSettings Prefabs { get { return S.Prefabs; } }

        protected override void Enter()
        {
            if (!Battle.Level.Actions.CanMove(Unit, Battle))
            {
                ChangeState(new PlayerState());
                return;
            }
            else if (!Unit.IsDead)
            {
                Walkable = Map.WalkableTiles(Unit.TilePosition, Unit.Mover.MoveRange);
                MoveRageShow(Unit.TilePosition, Unit.Mover.MoveRange);
                PathShow();
            }
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

            var plane = Map.Settings.Plane();
            var target = InputController.GroundPosition(plane);
            PathUpdate(target);

            if (InputController.GetOnWorldUp(plane, true))
            {
                var tile = Map.Tile(target);
                var position = Controller.Map.WorldPosition(tile);
                if (!Unit.IsDead && Walkable.Contains(tile))
                {
                    if (Unit.TilePosition != tile.Position)
                    {
                        MoveRangeHide();
                        PathHide();
                        ChangeState(new SpectatorState());
                        Battle.TurnPlayer.Play(new MoveAction(Battle.Player, Unit, position), () => ChangeState(new PlayerState()));
                        return;
                    }
                }
                else
                {
                    TileMarker.ShowInvalid(position);
                }
            }

        }

        public override void Exit()
        {
            MoveRangeHide();
            PathHide();
        }

        public override bool IsSaveable()
        {
            return Game.Loader != null;
        }

        /// <summary>
        /// Create path trajectory
        /// </summary>
        void PathShow()
        {
            if (!Path)
            {
                Path = Spawner.Spawn(Prefabs.Path, Vector3.zero, Quaternion.identity);
                Path.Show(new List<Vector3>() { });
                Path.InactiveState();
                Path.Init(Map);
            }
        }

        /// <summary>
        /// Hide path trajectory
        /// </summary>
        void PathHide()
        {
            if (Path)
            {
                Spawner.Despawn(Path.gameObject);
            }
        }

        /// <summary>
        /// Draw path trajectory from created origin to worldPosition
        /// </summary>
        /// <param name="worldPosition"></param>
        void PathUpdate(Vector3 worldPosition)
        {
            if (Path)
            {
                var tile = Map.Tile(worldPosition);
                if (Walkable.Contains(tile))
                {
                    var path = Map.PathPoints(Unit.WorldPosition, Map.WorldPosition(tile.Position), Unit.Mover.MoveRange);
                    Path.Show(path);
                    Path.ActiveState();
                    AreaOutline.ActiveState();
                }
                else
                {
                    Path.InactiveState();
                    AreaOutline.InactiveState();
                }
            }
        }

        /// <summary>
        /// Show area outline
        /// </summary>
        /// <param name="position">Center or outline (in tile coordinates)</param>
        /// <param name="range"></param>
        void MoveRageShow(Vector3Int position, float range)
        {
            MoveRangeHide();
            if (!AreaOutline)
            {
                AreaOutline = Spawner.Spawn(Prefabs.AreaOutline, Vector3.zero, Quaternion.identity);
            }
            AreaOutline.Show(Map.WalkableBorder(position, range), Map, AreaOffset);
        }

        /// <summary>
        /// Hide area outline
        /// </summary>
        void MoveRangeHide()
        {
            if (AreaOutline)
            {
                Spawner.Despawn(AreaOutline.gameObject);
            }
        }
    }
}