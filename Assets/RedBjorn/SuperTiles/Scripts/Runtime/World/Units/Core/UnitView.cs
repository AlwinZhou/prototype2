using RedBjorn.ProtoTiles;
using RedBjorn.Utils;
using System;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// View which represents UnitEntity at unity scene
    /// </summary>
    public class UnitView : MonoBehaviour
    {
        GameEntity Game;

        public UnitEntity Unit { get; private set; }
        public GameObject ColliderParent { get; set; }
        public GameObject Model { get; private set; }
        public InteractableGameobject Interactable { get; private set; }
        public TransformTagHolder[] TransformHolders { get; private set; }

        public Vector3Int Position
        {
            get
            {
                if (Map != null)
                {
                    var tile = Map.Tile(transform.position);
                    if (tile != null)
                    {
                        return tile.Position;
                    }
                    return Vector3Int.zero;
                }
                return Vector3Int.zero;
            }
        }

        public Quaternion Rotation { get { return transform.rotation; } }
        public Vector3 WorldPosition { get { return transform.position; } set { transform.position = value; } }
        MapEntity Map => Game.Battle.Map;

        public event Action<bool> OnStateChanged;

        void OnDestroy()
        {
            if (Unit != null)
            {
                Unit.OnDeadStateChanged -= OnDeadStateChanged;
            }
        }

        public void Init(GameEntity game, UnitEntity unit)
        {
            Game = game;
            
            if (Unit != null)
            {
                Unit.OnDeadStateChanged -= OnDeadStateChanged;
            }
            Unit = unit;
            if (Unit != null)
            {
                Unit.OnDeadStateChanged += OnDeadStateChanged;
            }

            name = $"Unit-{unit}";
            if (Unit.Data.Model)
            {
                Model = Spawner.Spawn(Unit.Data.Model, transform);
                Model.transform.localPosition = Vector3.zero;
                Model.transform.localRotation = Quaternion.identity;
                Model.name = "Model";
            }
            Interactable = gameObject.AddComponent<InteractableGameobject>();
            Interactable.Init(Game);
            TransformHolders = GetComponentsInChildren<TransformTagHolder>(true);
            var unitInit = GetComponentsInChildren<IUnitInitialize>(true);
            foreach (var c in unitInit)
            {
                c.Init(Unit);
            }
            var ui = GetTransformHolder(S.Battle.Tags.Transform.UiHolder);
            if (ui)
            {
                ui.localPosition = Unit.Data.UiHolder;
                var effectPrefab = S.Prefabs.EffectUI;
                if (effectPrefab)
                {
                    var effect = Spawner.Spawn(effectPrefab, ui);
                    effect.Init(Unit);
                }
            }
        }

        public void Show()
        {
            if (Model)
            {
                Model.SetActive(true);
            }
            OnStateChanged.SafeInvoke(true);
        }

        public void Hide()
        {
            if (Model)
            {
                Model.SetActive(false);
            }
            OnStateChanged.SafeInvoke(false);
        }

        public Transform GetTransformHolder(TransformTag tag)
        {
            var holder = TransformHolders.FirstOrDefault(t => t.Tag == tag);
            return holder ? holder.transform : null;
        }

        void OnDeadStateChanged(bool dead)
        {
            if (ColliderParent)
            {
                ColliderParent.SetActive(!dead);
            }
        }
    }
}
