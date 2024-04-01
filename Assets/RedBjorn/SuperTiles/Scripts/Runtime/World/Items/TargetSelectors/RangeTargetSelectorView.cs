using RedBjorn.ProtoTiles;
using RedBjorn.SuperTiles.Battle.Actions;
using RedBjorn.SuperTiles.Utils;
using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items.TargetSelectors
{
    /// <summary>
    /// View which represents RangeTargetSelector at unity scene and fills ItemAction as an output 
    /// </summary>
    public class RangeTargetSelectorView : MonoBehaviour, ITargetSelectorView
    {
        public GameObject RangeParent;
        public GameObject RangeRoot;
        public GameObject AvailableParent;
        public LineDrawer Trajectory;

        bool UseTrajectory;
        ItemAction Action;
        GameObject SelectorRef;
        RangeTargetSelector Logic;
        Action<ItemAction> OnCompleted;
        BattleEntity Battle;
        InteractableDetector Detector;
        List<UnitEntity> Targets = new List<UnitEntity>();
        List<GameObject> SelectorArea = new List<GameObject>();
        List<GameObject> AvailableArea = new List<GameObject>();

        const float TrajectoryOffset = 0.03f;
        const float RangeOffset = 0.02f;
        const float AvailableOffset = 0.01f;

        public MapEntity Map { get { return Battle.Map; } }

        void Update()
        {
            SelectorUpdate();
            Input();
        }

        public void Init(RangeTargetSelector selector, ItemAction action, Action<ItemAction> onCompleted, BattleView controller)
        {
            Action = action;
            Battle = controller.Battle;
            Detector = controller.Detector;
            Logic = selector;
            OnCompleted = onCompleted;
            UseTrajectory = Logic.ShowTrajectory;
            transform.position = Action.Unit.WorldPosition;

            SelectorCreate();
            AvailableCreate();
            TrajectoryCreate();

            if (Detector)
            {
                Detector.Pause();
            }
        }

        public void Abort()
        {
            TargetsClear();
            if (Detector)
            {
                Detector.Play();
            }
            AvailableClear();
            Spawner.Despawn(gameObject);
        }

        void SelectorCreate()
        {
            if (Action.Item.Data.Visual.UseSelectorCustom && Action.Item.Data.Visual.SelectorCustom.gameObject)
            {
                SelectorRef = Spawner.Spawn(Action.Item.Data.Visual.SelectorCustom.gameObject, Vector3.zero, Quaternion.identity);
            }
            if (!SelectorRef)
            {
                SelectorRef = Map.TileCreate(Action.Item.Data.Visual.SelectorGenerated);
            }

            SelectorRef.SetActive(true);
            SelectorRef.transform.SetParent(RangeRoot.transform);

            SelectorSpawn(Logic.AreaCapacity(Action.Item, Battle));

            SelectorRef.SetActive(false);

            RangeParent.transform.localPosition = Map.Settings.VectorCreateOrthogonal(RangeOffset);
        }

        void SelectorSpawn(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var obj = Spawner.Spawn(SelectorRef, SelectorRef.transform.parent);
                SelectorArea.Add(obj);
            }
        }

        void SelectorUpdate()
        {
            Action.Position = InputController.GroundPosition(Map.Settings.Plane());
            var tile = Logic.SelectTile(Action.Item, Action.Unit.WorldPosition, Action.Position, Action.Unit, Battle);
            if (tile == null)
            {
                SelectorArea.ForEach(s => s.SetActive(false));
                if (UseTrajectory)
                {
                    Trajectory.Hide();
                }
                TargetsClear();
            }
            else
            {
                var targetPosition = Map.WorldPosition(tile);
                RangeRoot.transform.position = Map.Settings.Projection(targetPosition, RangeOffset);
                var area = Logic.AreaSelectPositions(Action.Item, tile, Battle);
                if (area != null)
                {
                    for (int i = 0; i < area.Count; i++)
                    {
                        SelectorSpawn(area.Count - SelectorArea.Count);
                        SelectorArea[i].transform.position = Map.Settings.Projection(area[i], RangeOffset);
                        SelectorArea[i].SetActive(true);
                    }
                    for (int i = area.Count; i < SelectorArea.Count; i++)
                    {
                        SelectorArea[i].SetActive(false);
                    }

                    if (UseTrajectory)
                    {
                        var start = Map.Settings.Projection(transform.position);
                        var finish = Map.Settings.Projection(RangeRoot.transform.position);
                        Trajectory.Show(start, finish); 
                    }

                    var newTargets = Logic.SelectTargets(Action.Item, Action.Unit.WorldPosition, tile, Action.Unit, Battle);
                    foreach (var target in newTargets)
                    {
                        if (Targets.Contains(target))
                        {
                            Targets.Remove(target);
                        }
                        else
                        {
                            if (target.View.Interactable)
                            {
                                target.View.Interactable.StartInteracting();
                            }
                        }
                    }
                    foreach (var target in Targets)
                    {
                        if (target.View.Interactable)
                        {
                            target.View.Interactable.StopInteracting();
                        }
                    }
                    Targets = newTargets.ToList();
                }
                else
                {
                    SelectorArea.ForEach(s => s.SetActive(false));
                    if (UseTrajectory)
                    {
                        Trajectory.Hide();
                    }
                    TargetsClear();
                }
            }
        }

        void AvailableCreate()
        {
            GameObject tileRef = null;
            if (Action.Item.Data.Visual.UseAvailableCustom && Action.Item.Data.Visual.AvailableCustom)
            {
                tileRef = Spawner.Spawn(Action.Item.Data.Visual.AvailableCustom, Vector3.zero, Quaternion.identity);
            }
            if (!tileRef)
            {
                tileRef = Map.TileCreate(Action.Item.Data.Visual.AvailableGenerated);
                tileRef.SetActive(true);
            }

            foreach (var position in Logic.AreaAvailable(Action.Item, transform.position, Action.Unit, Battle))
            {
                var tile = Spawner.Spawn(tileRef, AvailableParent.transform);
                tile.transform.position = position;
                AvailableArea.Add(tile);
            }
            Spawner.Despawn(tileRef);

            AvailableParent.transform.localPosition = Map.Settings.VectorCreateOrthogonal(AvailableOffset);
        }

        void AvailableClear()
        {
            for (int i = AvailableArea.Count - 1; i >= 0; i--)
            {
                Spawner.Despawn(AvailableArea[i]);
            }
            AvailableArea.Clear();
        }

        void TrajectoryCreate()
        {
            Trajectory.transform.localPosition = Map.Settings.VectorCreateOrthogonal(TrajectoryOffset);
            Trajectory.Line.transform.localRotation = Map.Settings.RotationPlane();
            if (UseTrajectory)
            {
                Trajectory.Line.material = Action.Item.Data.Visual.TrajectoryMaterial;
            }
            else
            {
                Trajectory.Hide();
            }
        }

        void TargetsClear()
        {
            foreach (var t in Targets)
            {
                if (t.View.Interactable)
                {
                    t.View.Interactable.StopInteracting();
                }
            }
            Targets.Clear();
        }

        void Input()
        {
            if (InputController.GetOnWorldUp(Map.Settings.Plane(), true))
            {
                TargetsClear();
                OnCompleted.SafeInvoke(Action);
                if (Detector)
                {
                    Detector.Play();
                }
                AvailableClear();
                Spawner.Despawn(gameObject);
            }
        }
    }
}
