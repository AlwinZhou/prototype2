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
    /// View which represents DirectionTargetSelector at unity scene and fills ItemAction as an output 
    /// </summary>
    public class DirectionTargetSelectorView : MonoBehaviour, ITargetSelectorView
    {
        public Transform RangeParent;
        public LineDrawer Trajectory;

        ItemAction Action;
        GameObject SelectorRef;
        DirectionTargetSelector Logic;
        Action<ItemAction> OnCompleted;
        BattleEntity Battle;
        InteractableDetector Detector;
        List<UnitEntity> Targets = new List<UnitEntity>();
        List<TileEntity> Area = new List<TileEntity>();
        List<GameObject> SelectorArea = new List<GameObject>();

        const float TrajectoryOffset = 0.02f;
        const float RangeOffset = 0.01f;

        MapEntity Map { get { return Battle.Map; } }

        void Update()
        {
            SelectorUpdate();
            Input();
        }

        public void Init(ItemAction action, DirectionTargetSelector selector, Action<ItemAction> onCompleted, BattleView controller)
        {
            Action = action;
            Battle = controller.Battle;
            Detector = controller.Detector;
            OnCompleted = onCompleted;
            Logic = selector;
            transform.position = Action.Unit.WorldPosition;

            SelectorCreate();
            TrajectoryCreate();

            if (Detector)
            {
                Detector.Pause();
            }
        }

        public void Abort()
        {
            TargetsClear();
            Detector.Play();
            Spawner.Despawn(gameObject);
        }

        void SelectorCreate()
        {
            RangeParent.parent.transform.localPosition = Map.Settings.VectorCreateOrthogonal(RangeOffset);
            if (Action.Item.Data.Visual.UseSelectorCustom && Action.Item.Data.Visual.SelectorCustom.gameObject)
            {
                SelectorRef = Spawner.Spawn(Action.Item.Data.Visual.SelectorCustom.gameObject, Vector3.zero, Quaternion.identity);
            }
            if (!SelectorRef)
            {
                SelectorRef = Map.TileCreate(Action.Item.Data.Visual.SelectorGenerated);
            }
            SelectorRef.transform.SetParent(RangeParent);
            SelectorRef.SetActive(true);
            for (int i = 0; i < 2 * Action.Item[Logic.StatRange]; i++)
            {
                var s = Spawner.Spawn(SelectorRef, RangeParent);
                s.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                SelectorArea.Add(s);
                s.SetActive(false);
            }
            SelectorRef.SetActive(false);
        }

        void SelectorUpdate()
        {
            Action.Position = Logic.NormalizeTarget(Action.Item, Action.Unit.WorldPosition, InputController.GroundPosition(Map.Settings.Plane()), Battle);
            Area = Logic.TargetTiles(Action.Item, transform.position, Action.Position, Action.Unit, Battle);
            if (Area == null)
            {
                SelectorArea.ForEach(s => s.SetActive(false));
                if (Logic.ShowTrajectory)
                {
                    Trajectory.Hide();
                }
                TargetsClear();
            }
            else
            {
                for (int i = 0; i < Area.Count; i++)
                {
                    SelectorArea[i].transform.position = Map.Settings.Projection(Map.WorldPosition(Area[i]), RangeOffset);
                    SelectorArea[i].SetActive(true);
                }
                for (int i = Area.Count; i < SelectorArea.Count; i++)
                {
                    SelectorArea[i].SetActive(false);
                }

                if (Logic.ShowTrajectory)
                {
                    var start = Map.Settings.Projection(transform.position);
                    var finish = Map.Settings.Projection(Action.Position);
                    Trajectory.Show(start, finish);
                }

                var newTargets = Logic.SelectTargets(Action.Item, Area, Action.Unit, Battle);
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
        }

        void TrajectoryCreate()
        {
            Trajectory.transform.localPosition = Map.Settings.VectorCreateOrthogonal(TrajectoryOffset);
            Trajectory.Line.transform.localRotation = Map.Settings.RotationPlane();
            if (Logic.ShowTrajectory)
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
                Detector.Play();
                Spawner.Despawn(gameObject);
            }
        }
    }
}