using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RedBjorn.ProtoTiles;
using RedBjorn.Utils;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Level informational storage
    /// </summary>
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Level.Asset)]
    public class LevelData : ScriptableObjectExtended
    {
        [Serializable]
        public class CameraData
        {
            public Vector3 StartPosition;
            public Vector3 StartRotation;
            public GameObject Prefab;
        }

        [Serializable]
        public class BattleStartData
        {
            public bool AddResources;
            public ResourceTag Resource;
            public int ResourceValue;
        }

        [Serializable]
        public class TurnStartData
        {
            public bool AddResources;
            public ResourceTag Resource;
            public UnitTag ResourceCountUnit;
            public int ResourceValue;
            [Space]
            public bool HealAllyInTile;
            public UnitData HealAllyInTileHealAllExcept;
            public UnitData HealAllyInTileHealer;
            public float HealAllyInTeamAmount;
            [Space]
            public bool HealSelf;
            public UnitData HealSelfTarget;
            public UnitData HealSelfBlocker;
            public float HealSelfAmount;
        }

        [Serializable]
        public class TurnFinishData
        {

        }

        public string Caption;
        public string SceneName;
        [Tooltip("Should battle start after level load")]
        public bool AutoStart = true;
        [Tooltip("When same effect is added, should it only increase effect duration")]
        public bool EqualEffectInfluenceDuration = true;
        public bool TeamMarkersEnable = true;
        public MapSettings Map;
        public TurnResolver Turn;
        public ActionRules Actions;
        public HealthConvertRules Health;
        public BattleFinishHandler BattleFinish;
        public RelationshipTable Relationship;
        public ProtoTiles.Tiles.TileCondition TileRuleIsItemAvailable;
        public GameObject MarkerAlly;
        public GameObject MarkerEnemy;
        public CameraData Camera;
        public List<SquadControllerData> Players;
        public BattleStartData OnBattleStartAction;
        public TurnStartData OnTurnStartAction;
        public TurnFinishData OnTurnFinishAction;

        public IEnumerable<SquadControllerEntity> AllyPlayers(SquadControllerEntity controller, BattleEntity battle)
        {
            if (controller != null && battle != null)
            {
                RelationshipTable.Info info = null;
                if (Relationship)
                {
                    info = Relationship.Table.FirstOrDefault(t => t.Team == controller.Team);
                }
                if (info != null)
                {
                    foreach(var player in battle.Players.Where(p => info.Allies.Contains(p.Team) || controller.Team == p.Team))
                    {
                        yield return player;
                    }
                }
                else
                {
                    foreach(var player in battle.Players.Where(p => p.Team == controller.Team))
                    {
                        yield return player;
                    }
                }
            }
        }

        public IEnumerable<SquadControllerEntity> EnemyPlayers(SquadControllerEntity controller, BattleEntity battle)
        {
            if (controller != null && battle != null)
            {
                RelationshipTable.Info info = null;
                if (Relationship)
                {
                    info = Relationship.Table.FirstOrDefault(t => t.Team == controller.Team);
                }

                if (info != null)
                {
                    foreach (var player in battle.Players.Where(p => info.Enemies.Contains(p.Team)))
                    {
                        yield return player;
                    }
                }
                else
                {
                    foreach (var player in battle.Players.Where(p => p.Team != controller.Team))
                    {
                        yield return player;
                    }
                }
            }
        }

        public IEnumerable<UnitEntity> AllyUnits(SquadControllerEntity controller, BattleEntity battle)
        {
            foreach(var player in AllyPlayers(controller, battle))
            {
                foreach(var unit in player.Squad)
                {
                    yield return unit;
                }
            }
        }

        public IEnumerable<UnitEntity> EnemyUnits(SquadControllerEntity controller, BattleEntity battle)
        {
            foreach (var player in EnemyPlayers(controller, battle))
            {
                foreach (var unit in player.Squad)
                {
                    yield return unit;
                }
            }
        }

        public bool IsAllies(SquadControllerEntity main, SquadControllerEntity secondary)
        {
            if(main == null || secondary == null)
            {
                return false;
            }
            return Relationship != null ? Relationship.IsAlly(main.Team, secondary.Team) : false;
        }

        public bool IsEnemies(SquadControllerEntity main, SquadControllerEntity secondary)
        {
            if (main == null || secondary == null)
            {
                return false;
            }
            return Relationship != null ? Relationship.IsEnemy(main.Team, secondary.Team) : true;
        }

        public bool ItemAvailable(TileEntity tile)
        {
            if(tile == null)
            {
                return false;
            }
            if(TileRuleIsItemAvailable == null)
            {
                return tile.Vacant;
            }
            return TileRuleIsItemAvailable.IsMet(tile);
        }

        public IBattleAction GetBattleStartAction()
        {
            var action = new BattleActions.Multiple() { List = new List<IBattleAction>() };
            if (OnBattleStartAction != null && OnBattleStartAction.AddResources)
            {
                var resource = new BattleActions.ResourcesControllerAdd();
                resource.ControllerSelector = new SquadControllerSelectors.All();
                resource.Resource = OnBattleStartAction.Resource;
                resource.ResourceAmount = new FloatProviders.Constant() { Value = OnBattleStartAction.ResourceValue };
                action.List.Add(resource);
            }
            return action;
        }

        public IBattleAction GetTurnStartAction()
        {
            var action = new BattleActions.Multiple() { List = new List<IBattleAction>() };
            if (OnTurnStartAction != null)
            {
                if (OnTurnStartAction.AddResources)
                {
                    var resource = new BattleActions.ResourcesControllerAdd();
                    resource.ControllerSelector = new SquadControllerSelectors.Current();
                    resource.Resource = OnTurnStartAction.Resource;
                    resource.ResourceAmount = new FloatProviders.Multiply()
                    {
                        First = new FloatProviders.Constant() { Value = OnTurnStartAction.ResourceValue },
                        Second = new FloatProviders.UnitsCountContainsTag()
                        {
                            Tag = OnTurnStartAction.ResourceCountUnit,
                            Unit = new UnitSelectors.PlayerCurrentSquad()
                        }
                    };
                    action.List.Add(resource);
                }
            }
            if (OnTurnStartAction.HealAllyInTile)
            {
                var heal = new BattleActions.UnitHealOtherUnit()
                {
                    Amount = new FloatProviders.Constant() { Value = OnTurnStartAction.HealAllyInTeamAmount },
                    Target = new UnitSelectors.Filter()
                    {
                        Selector = new UnitSelectors.PlayerCurrentSquad(),
                        UnitFilter = new UnitFilters.Where()
                        {
                            Condition = new UnitConditions.And()
                            {
                                List = new List<IUnitCondition>()
                                {
                                    new UnitConditions.Not { Condition = new UnitConditions.DataEqual { Unit = OnTurnStartAction.HealAllyInTileHealAllExcept } },
                                    new UnitConditions.HealthLack(),
                                    new UnitConditions.Tile.UnitsSpecificAlly() 
                                    { 
                                        SpecificUnit = new UnitConditions.DataEqual { Unit = OnTurnStartAction.HealAllyInTileHealer } 
                                    }
                                }
                            }
                        }
                    }
                };

                action.List.Add(heal);
            }
            if (OnTurnStartAction.HealSelf)
            {
                var heal = new BattleActions.UnitHealOtherUnit()
                {
                    Amount = new FloatProviders.Constant() { Value = OnTurnStartAction.HealSelfAmount },
                    Target = new UnitSelectors.Filter()
                    {
                        Selector = new UnitSelectors.PlayerCurrentSquad(),
                        UnitFilter = new UnitFilters.Where()
                        {
                            Condition = new UnitConditions.And()
                            {
                                List = new List<IUnitCondition>()
                                {
                                    new UnitConditions.DataEqual { Unit = OnTurnStartAction.HealSelfTarget },
                                    new UnitConditions.HealthLack(),
                                    new UnitConditions.Tile.UnitsSpecificNotEnemy()
                                    {
                                        SpecificUnit =  OnTurnStartAction.HealSelfBlocker
                                                        ? new UnitConditions.DataEqual { Unit = OnTurnStartAction.HealSelfBlocker }
                                                        : null
                                    }
                                }
                            }
                        }
                    }
                };

                action.List.Add(heal);
            }
            return action;
        }

        public IBattleAction GetTurnFinishAction()
        {
            return null; // TODO
        }
#if UNITY_EDITOR
        public const string Suffix = "Level";

        public static LevelData Create(string directory, 
                                       string levelName, 
                                       MapSettings map, 
                                       TurnResolver turn, 
                                       ActionRules actions, 
                                       HealthConvertRules health,
                                       ProtoTiles.Tiles.TileCondition tileRuleIsItemAvailable)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var levelPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(directory, string.Concat(levelName, "_", Suffix, FileFormat.Asset)));
                var level = ScriptableObject.CreateInstance<LevelData>();
                level.Caption = levelName;
                level.AutoStart = true;
                level.EqualEffectInfluenceDuration = true;
                level.Map = map;
                level.Turn = turn;
                level.Actions = actions;
                level.Health = health;
                level.TileRuleIsItemAvailable = tileRuleIsItemAvailable;
                level.Camera = new CameraData();
                level.Players = new List<SquadControllerData>
                {
                    new SquadControllerData
                    {
                        Name = "Player",
                        Team = Tag.Find<TeamTag>("1"),
                        ControlledBy = SquadControllerType.Player
                    },
                    new SquadControllerData
                    {
                        Name = "Ai",
                        Team = Tag.Find<TeamTag>("2"),
                        ControlledBy = SquadControllerType.AI
                    }
                };

                AssetDatabase.CreateAsset(level, levelPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Log.I($"New level was created at path: {levelPath}");
                return level;
            }
            catch (Exception e)
            {
                Log.E(e);
                return null;
            }
        }

        public static LevelData Duplicate(LevelData level, string folder, string filename)
        {
            LevelData newLevel = null;
            if (!level)
            {
                return newLevel;
            }
            try
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var path = string.Format("{0}/{1}_{2}{3}", folder, filename, LevelData.Suffix, FileFormat.Asset);
                var levelPath = AssetDatabase.GenerateUniqueAssetPath(path);
                if (string.IsNullOrEmpty(levelPath))
                {
                    throw new Exception($"Could not genereate unique path for {path}");
                }

                string mapPath = null;
                if (level.Map)
                {
                    path = MapSettings.Path(folder, filename, level.Map.Type.ToString());
                    mapPath = AssetDatabase.GenerateUniqueAssetPath(path);
                    if (string.IsNullOrEmpty(mapPath))
                    {
                        throw new Exception($"Could not genereate unique path for {path}");
                    }
                }

                path = string.Format("{0}/{1}_{2}{3}", folder, filename, TurnResolver.Suffix, FileFormat.Asset);
                var turnPath = AssetDatabase.GenerateUniqueAssetPath(path);
                if (string.IsNullOrEmpty(turnPath))
                {
                    throw new Exception($"Could not genereate unique path for {path}");
                }

                path = string.Format("{0}/{1}_{2}{3}", folder, filename, ActionRules.Suffix, FileFormat.Asset);
                var actionsPath = AssetDatabase.GenerateUniqueAssetPath(path);
                if (string.IsNullOrEmpty(actionsPath))
                {
                    throw new Exception($"Could not genereate unique path for {path}");
                }

                path = string.Format("{0}/{1}_{2}{3}", folder, filename, HealthConvertRules.Suffix, FileFormat.Asset);
                var healthPath = AssetDatabase.GenerateUniqueAssetPath(path);
                if (string.IsNullOrEmpty(healthPath))
                {
                    throw new Exception($"Could not genereate unique path for {path}");
                }

                path = string.Format("{0}/{1}_{2}{3}", folder, filename, BattleFinishHandler.Suffix, FileFormat.Asset);
                var battleFinishPath = AssetDatabase.GenerateUniqueAssetPath(path);
                if (string.IsNullOrEmpty(battleFinishPath))
                {
                    throw new Exception($"Could not genereate unique path for {path}");
                }

                path = string.Format("{0}/{1}_{2}{3}", folder, filename, RelationshipTable.Suffix, FileFormat.Asset);
                var relationshipPath = AssetDatabase.GenerateUniqueAssetPath(path);
                if (string.IsNullOrEmpty(relationshipPath))
                {
                    throw new Exception($"Could not genereate unique path for {path}");
                }

                newLevel = Object.Instantiate(level);
                AssetDatabase.CreateAsset(newLevel, levelPath);

                if (level.Map)
                {
                    MapSettings newMap = Object.Instantiate(level.Map);
                    newLevel.Map = newMap;
                    AssetDatabase.CreateAsset(newLevel.Map, mapPath);
                }

                if (level.Turn)
                {
                    TurnResolver newTurn = Object.Instantiate(level.Turn);
                    newLevel.Turn = newTurn;
                    AssetDatabase.CreateAsset(newLevel.Turn, turnPath);
                }

                if (level.Actions)
                {
                    ActionRules newActions = Object.Instantiate(level.Actions);
                    newLevel.Actions = newActions;
                    AssetDatabase.CreateAsset(newLevel.Actions, actionsPath);
                }

                if (level.Health)
                {
                    HealthConvertRules newHealth = Object.Instantiate(level.Health);
                    newLevel.Health = newHealth;
                    AssetDatabase.CreateAsset(newLevel.Health, healthPath);
                }

                if (level.BattleFinish)
                {
                    BattleFinishHandler newBattleFinish = Object.Instantiate(level.BattleFinish);
                    newLevel.BattleFinish = newBattleFinish;
                    AssetDatabase.CreateAsset(newLevel.BattleFinish, battleFinishPath);
                }

                if (level.Relationship)
                {
                    RelationshipTable newRelationship = Object.Instantiate(level.Relationship);
                    newLevel.Relationship = newRelationship;
                    AssetDatabase.CreateAsset(newLevel.Relationship, relationshipPath);
                }

                AssetDatabase.SaveAssets();
                Log.I($"Level was to duplicated to {levelPath}");
                return newLevel;
            }
            catch (Exception e)
            {
                Log.E($"Level duplication failed. {e.Message}");
                return newLevel;
            }
            finally
            {
                AssetDatabase.Refresh();

            }
        }

        public static LevelData Find(string sceneName)
        {
            var guids = AssetDatabase.FindAssets(string.Concat("t:", typeof(LevelData).Name));
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var level = AssetDatabase.LoadAssetAtPath<LevelData>(path);
                if (level && !string.IsNullOrEmpty(level.SceneName) && level.SceneName.Equals(sceneName))
                {
                    return level;
                }
            }
            return null;
        }

        public static LevelData[] FindAll()
        {
            var guids = AssetDatabase.FindAssets(string.Concat("t:", typeof(LevelData).Name));
            var levels = new LevelData[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                levels[i] = AssetDatabase.LoadAssetAtPath<LevelData>(path);
            }
            return levels;
        }
#endif
    }
}
