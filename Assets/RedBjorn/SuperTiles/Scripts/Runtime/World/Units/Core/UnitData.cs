using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using RedBjorn.Utils;
using System;
using System.IO;
using System.Linq;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Unit informational storage
    /// </summary>
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Unit.Asset)]
    public class UnitData : ScriptableObjectExtended
    {
        [Serializable]
        public class StatData
        {
            public UnitStatTag Stat;
            public float Value;
        }

        [Serializable]
        public class DiedActionData
        {
            public bool Rebirth;
            public bool KillerTakeControl;
        }

        [Serializable]
        public class UnitChangeControllerData
        {
            public bool ChangeModelColorData;
            public List<UnitChangeControllerActions.ModelColorChange.Data> ModelColorData;
        }

        [Serializable]
        public class HealthActionData 
        {
            public bool HaveHealthCustomPrefab;
            public GameObject HealthCustomPrefab;
        }


        public Sprite Avatar;
        public GameObject Model;
        public GameObject Collider;
        public Vector3 UiHolder;
        public List<UnitTag> Tags;
        public List<StatData> Stats;
        public List<ItemData> DefaultItems;
        public HealthActionData HealthAction;
        public DiedActionData DiedAction;
        public UnitChangeControllerData UnitChangeControllerAction;

        public static UnitData Find(string filter)
        {
#if UNITY_EDITOR
            var guids = AssetDatabase.FindAssets($"t: {typeof(UnitData).Name} {filter}");
            if (guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                var data = AssetDatabase.LoadAssetAtPath<UnitData>(path);
                return data;
            }
#endif
            return null;
        }

        public UnitView CreateView(Vector3 position, Quaternion rotation)
        {
            var view = Spawner.Spawn(S.Prefabs.UnitView);
            view.transform.position = position;
            view.transform.rotation = rotation;
            GameObject colliderGo = null;
            if (Collider)
            {
                colliderGo = Spawner.Spawn(Collider);
                colliderGo.name = "Collider";
            }
            else
            {
                colliderGo = new GameObject("Collider");
                var collider = colliderGo.AddComponent<BoxCollider>();
                collider.center = new Vector3(0, 0.28f, 0f);
                collider.size = new Vector3(0.32f, 0.56f, 0.52f);
            }

            colliderGo.transform.SetParent(view.transform);
            colliderGo.transform.localPosition = Vector3.zero;
            colliderGo.transform.localRotation = Quaternion.identity;
            view.ColliderParent = colliderGo;

            var link = view.ColliderParent.AddComponent<InteractableGameobjectLink>();
            link.Rerefence = view.gameObject;

            return view;
        }

        public IUnitAction GetHealthAction()
        {
            if (HealthAction.HaveHealthCustomPrefab)
            {
                return new UnitActions.HealthViewCreatePrefab() { HealthPrefab = HealthAction.HealthCustomPrefab };
            }
            return new UnitActions.HealthViewCreateDefault();
        }

        public IDiedAction GetDiedAction()
        {
            var action = new DiedActions.Multiple() { List = new List<IDiedAction>() };
            if (DiedAction.Rebirth) 
            {
                action.List.Add(new DiedActions.Rebirth());
            }
            if (DiedAction.KillerTakeControl) 
            {
                action.List.Add(new DiedActions.KillerTakeControl());
            }
            if(action.List.Count == 0)
            {
                action.List.Add(new DiedActions.VictimMapUnregister());
            }
            return action;
        }

        public IUnitChangeControllerAction GetUnitChangeControllerAction()
        {
            var action = new UnitChangeControllerActions.Multiple() { List = new List<IUnitChangeControllerAction>() };
            if (UnitChangeControllerAction.ChangeModelColorData && UnitChangeControllerAction.ModelColorData != null)
            {
                var data = new UnitChangeControllerActions.ModelColorChange() 
                { 
                    Info = new List<UnitChangeControllerActions.ModelColorChange.Data>(UnitChangeControllerAction.ModelColorData) 
                };
                action.List.Add(data);
            }
            return action;
        }

#if UNITY_EDITOR
        public static UnitData Create(string folderPath, string name, List<UnitData.StatData> stats)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var unit = ScriptableObject.CreateInstance<UnitData>();
                unit.Stats = stats;
                var unitPathUnique = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(folderPath, string.Concat(name, FileFormat.Asset)));

                AssetDatabase.CreateAsset(unit, unitPathUnique);
                EditorUtility.SetDirty(unit);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Log.I($"New unit was created at path: {unitPathUnique}");

                return unit;
            }
            catch (Exception e)
            {
                Log.E(e);
                return null;
            }
        }
#endif
    }
}