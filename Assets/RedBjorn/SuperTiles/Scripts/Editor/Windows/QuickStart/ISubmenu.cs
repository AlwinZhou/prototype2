using RedBjorn.ProtoTiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.QuickStart
{
    public interface ISubmenu
    {
        string Caption();
        void Show(QuickStartWindow window);
    }

    public class Multiple : ISubmenu
    {
        public List<ISubmenu> Menus;
        public string[] TabsCaption;
        public int Tab;

        public Multiple(List<ISubmenu> menus)
        {
            Menus = menus;
            TabsCaption = Menus.Select(s => s.Caption()).ToArray();
        }

        public string Caption()
        {
            return string.Join(",", TabsCaption);
        }

        public void Show(QuickStartWindow window)
        {
            window.Tabs.x = 2 * window.Border;
            window.Tabs.y = 2 * window.Border;
            window.Tabs.width = window.WindowWidth - 4 * window.Border;
            window.Tabs.height = window.TabsHeight;
            EditorGUI.DrawRect(window.Tabs, window.Settings.MenuColor);

            window.Common.x = window.Tabs.x;
            window.Common.y = window.Tabs.height + 2 * window.Border;
            window.Common.width = window.WindowWidth - 4 * window.Border;
            window.Common.height = window.WindowHeight - (window.NavigationHeight + 2 * window.Border) - (window.TabsHeight + 2 * window.Border) - 2 * window.Border;
            EditorGUI.DrawRect(window.Common, window.Settings.CommonColor);
            window.CommonContent.x = window.Common.x + 2 * window.Border;
            window.CommonContent.y = window.Common.y + 2 * window.Border;
            window.CommonContent.width = window.Common.width - 4 * window.Border;
            window.CommonContent.height = window.Common.height - 4 * window.Border;

            window.Navigation.x = window.WindowWidth - window.NavigationWidth - 2 * window.Border;
            window.Navigation.y = window.WindowHeight - window.NavigationHeight - 2 * window.Border;
            window.Navigation.width = window.NavigationWidth;
            window.Navigation.height = window.NavigationHeight;

            GUILayout.BeginArea(window.Tabs);
            Tab = GUILayout.SelectionGrid(Tab, TabsCaption, TabsCaption.Length);
            GUILayout.EndArea();

            GUILayout.BeginArea(window.CommonContent);
            GUILayout.Space(20);
            Menus[Tab].Show(window);
            GUILayout.EndArea();

            GUILayout.BeginArea(window.Navigation);
            GUILayout.BeginHorizontal();
            if (Tab > 0)
            {
                if (GUILayout.Button("<<"))
                {
                    Tab = Mathf.Max(Tab - 1, 0);
                }
            }
            if (Tab < TabsCaption.Length - 1)
            {

                if (Tab == 0)
                {
                    GUILayout.Space(window.Navigation.width / 2);
                }
                if (GUILayout.Button(">>"))
                {
                    Tab = Mathf.Min(Tab + 1, TabsCaption.Length - 1);
                }
            }
            else
            {
                var enabled = GUI.enabled;
                if (GUILayout.Button("Done"))
                {
                    window.Close();
                }
                GUI.enabled = enabled;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }

    public class SceneSubmenu : ISubmenu
    {
        QuickStartWindow Window;
        int AxisIndex;
        string[] Axes;
        int GridIndex;
        string[] Grids;
        int TurnResolverIndex;
        string[] TurnResolvers;
        int ActionIndex;
        string[] ActionRules;
        int BattleFinishIndex;
        string[] BattleFinishers;
        int AiTypeIndex;
        string[] AiTypes;
        string UnitFilename;
        string AiFilename;

        bool Creating;


        public SceneSubmenu()
        {
            AxisIndex = 0;
            Axes = Enum.GetNames(typeof(GridAxis));

            GridIndex = 0;
            Grids = Enum.GetNames(typeof(GridType));

            TurnResolverIndex = 0;
            TurnResolvers = typeof(TurnResolver).Assembly.GetTypes()
                                                           .Where(t => t.IsClass && t.IsSubclassOf(typeof(TurnResolver)))
                                                           .Select(t => t.Name)
                                                           .ToArray();

            ActionIndex = 0;
            ActionRules = typeof(ActionRules).Assembly.GetTypes()
                                                           .Where(t => t.IsClass && t.IsSubclassOf(typeof(ActionRules)))
                                                           .Select(t => t.Name)
                                                           .ToArray();

            BattleFinishIndex = 0;
            BattleFinishers = typeof(BattleFinishHandler).Assembly.GetTypes()
                                                                .Where(t => t.IsClass && t.IsSubclassOf(typeof(BattleFinishHandler)))
                                                                .Select(t => t.Name)
                                                                .ToArray();

            AiTypeIndex = 0;
            AiTypes = typeof(UnitAiData).Assembly.GetTypes()
                                                           .Where(t => t.IsClass && t.IsSubclassOf(typeof(UnitAiData)))
                                                           .Select(t => t.Name)
                                                           .ToArray();
            AiFilename = "Ai";
            UnitFilename = "Unit";
        }

        public string Caption()
        {
            return "Scene";
        }

        public void Show(QuickStartWindow window)
        {
            Window = window;
            if (Creating)
            {
                EditorGUILayout.LabelField("Wait: Scene creation process");
                return;
            }
            EditorGUILayout.LabelField("Scene", EditorStyles.boldLabel);
            window.FolderPath = EditorGUILayout.TextField("Folder:", window.FolderPath);
            window.SceneName = EditorGUILayout.TextField("Scene Filename:", window.SceneName);

            GUILayout.Space(20);
            EditorGUILayout.LabelField("Map", EditorStyles.boldLabel);
            GridIndex = EditorGUILayout.Popup("Grid:", GridIndex, Grids);
            AxisIndex = EditorGUILayout.Popup("Axis:", AxisIndex, Axes);
            ActionIndex = EditorGUILayout.Popup("Action Rules:", ActionIndex, ActionRules);
            TurnResolverIndex = EditorGUILayout.Popup("Turn Resolver:", TurnResolverIndex, TurnResolvers);
            BattleFinishIndex = EditorGUILayout.Popup("Battle Finish:", BattleFinishIndex, BattleFinishers);

            GUILayout.Space(20);
            EditorGUILayout.LabelField("Unit", EditorStyles.boldLabel);
            UnitFilename = EditorGUILayout.TextField("Unit Filename:", UnitFilename);
            AiFilename = EditorGUILayout.TextField("Ai Filename:", AiFilename);
            AiTypeIndex = EditorGUILayout.Popup("Ai Logic:", AiTypeIndex, AiTypes);
            if (GUILayout.Button("Create"))
            {
                Creating = true;
                try
                {
                    if (!Directory.Exists(window.FolderPath))
                    {
                        Directory.CreateDirectory(window.FolderPath);
                    }

                    EditorSceneManager.newSceneCreated += OnNewSceneCreated;
                    var mapSettings = MapWindowSettings.Instance;
                    var axis = (GridAxis)Enum.Parse(typeof(GridAxis), Axes[AxisIndex]);
                    var grid = (GridType)Enum.Parse(typeof(GridType), Grids[GridIndex]);
                    var map = MapSettings.Create(Window.FolderPath, Window.SceneName, grid, axis, mapSettings.Rules, mapSettings.CellBorder);
                    var turn = TurnResolver.Create(Window.FolderPath, Window.SceneName, TurnResolvers[TurnResolverIndex]);
                    var actions = RedBjorn.SuperTiles.ActionRules.Create(Window.FolderPath, Window.SceneName, ActionRules[ActionIndex]);
                    var health = HealthConvertRules.Create(Window.FolderPath, Window.SceneName);
                    var level = LevelData.Create(Window.FolderPath, Window.SceneName, map, turn, actions, health, Window.Settings.TileIsItemAvailableRule);
                    level.BattleFinish = BattleFinishHandler.Create(Window.FolderPath, Window.SceneName, BattleFinishers[BattleFinishIndex]);
                    level.Relationship = RelationshipTable.Create(Window.FolderPath, Window.SceneName);
                    if (level.Relationship)
                    {
                        level.Relationship.SetDefault(level.Players);
                    }
                    level.SceneName = Window.SceneName;
                    window.Level = level;
                    window.CachedUnit = UnitData.Create(Window.UnitFolderPath, UnitFilename, S.Battle.Tags.Unit.GetDefault());
                    window.CachedUnitAi = UnitAiData.Create(Window.UnitAiFolderPath, AiFilename, AiTypes[AiTypeIndex]);
                    EditorUtility.SetDirty(level);
                    AssetDatabase.SaveAssets();

                    var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                }
                catch (Exception e)
                {
                    Creating = false;
                    Log.E(e);
                }
            }
        }

        void OnNewSceneCreated(UnityEngine.SceneManagement.Scene scene, NewSceneSetup setup, NewSceneMode mode)
        {
            EditorSceneManager.newSceneCreated -= OnNewSceneCreated;

            MenuLoader.Create();

            var light = new GameObject("Directional Light");
            light.transform.rotation = Quaternion.Euler(60f, 0f, 0f);
            var pointLight = light.AddComponent<Light>();
            pointLight.type = LightType.Directional;

            MapWindow.CreateHolder();

            EditorSceneManager.SaveScene(scene, Window.ScenePath);

            if (S.Levels)
            {
                S.Levels.Update();
            }

            Creating = false;
            Window.Tab = 1;
        }
    }

    public class LevelSubmenu : ISubmenu
    {
        public string Caption()
        {
            return "Level";
        }

        public void Show(QuickStartWindow window)
        {
            GUI.enabled = false;
            window.Level = EditorGUILayout.ObjectField("Level Asset", window.Level, typeof(LevelData), allowSceneObjects: false) as LevelData;
            GUI.enabled = true;
            if (GUILayout.Button("Edit"))
            {
                LevelWindow.DoShow(window.Level);
            }
        }
    }

    public class UnitSpawnSubmenu : ISubmenu
    {
        public string Caption()
        {
            return "Unit Spawn";
        }

        public void Show(QuickStartWindow window)
        {
            window.CachedTeam = EditorGUILayout.ObjectField("Team", window.CachedTeam, typeof(TeamTag), allowSceneObjects: false) as TeamTag;
            EditorGUILayout.BeginHorizontal();
            window.CachedUnit = EditorGUILayout.ObjectField("Unit", window.CachedUnit, typeof(UnitData), allowSceneObjects: false) as UnitData;
            GUI.enabled = window.CachedUnit;
            if (GUILayout.Button("Edit", GUILayout.Width(100f)))
            {
                UnitWindow.DoShow(window.CachedUnit);
            }
            EditorGUILayout.EndHorizontal();

            GUI.enabled = true;
            EditorGUILayout.BeginHorizontal();
            window.CachedUnitAi = EditorGUILayout.ObjectField("Ai", window.CachedUnitAi, typeof(UnitAiData), allowSceneObjects: false) as UnitAiData;
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Create"))
            {
                var existed = ObjectExtensions.FindObjectOfType<UnitSpawnPoint>();
                Transform spawnpoints;
                if (existed)
                {
                    spawnpoints = existed.transform.parent;
                }
                else
                {
                    spawnpoints = new GameObject("SpawnPoints").transform;
                }

                var point = UnitSpawnPoint.Create("Spawnpoint (1)", spawnpoints, Vector3.zero, Quaternion.identity, window.CachedTeam, window.CachedUnit, window.CachedUnitAi);
                Selection.activeTransform = point.transform;
                EditorSceneManager.SaveOpenScenes();
            }
        }
    }

    public class MapSubmenu : ISubmenu
    {
        public string Caption()
        {
            return "Map";
        }

        public void Show(QuickStartWindow window)
        {
            GUI.enabled = false;
            var map = window.Level ? window.Level.Map : null;
            map = EditorGUILayout.ObjectField("Map Asset", map, typeof(MapSettings), allowSceneObjects: false) as MapSettings;
            GUI.enabled = true;
            if (GUILayout.Button("Edit"))
            {
                MapWindow.DoShow(map);
            }
        }
    }
}
