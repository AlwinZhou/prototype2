using RedBjorn.ProtoTiles;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace RedBjorn.SuperTiles.Editors.Level.Submenus
{
    public class Create : ISubmenu
    {
        public Rect Common;
        public Rect CommonContent;
        public Rect WorkArea;
        public Rect WorkAreaContent;

        int AxisIndex;
        string[] Axes;
        int GridIndex;
        string[] Grids;
        int ActionIndex;
        string[] ActionRules;
        int ResolverIndex;
        string[] TurnResolvers;
        int BattleFinishIndex;
        string[] BattleFinishers;

        public Create()
        {
            ResolverIndex = 0;
            TurnResolvers = typeof(TurnResolver).Assembly.GetTypes()
                                                           .Where(t => t.IsClass && t.IsSubclassOf(typeof(TurnResolver)))
                                                           .Select(t => t.Name)
                                                           .ToArray();
            BattleFinishIndex = 0;
            BattleFinishers = typeof(BattleFinishHandler).Assembly.GetTypes()
                                                                .Where(t => t.IsClass && t.IsSubclassOf(typeof(BattleFinishHandler)))
                                                                .Select(t => t.Name)
                                                                .ToArray();
            AxisIndex = 0;
            Axes = Enum.GetNames(typeof(GridAxis));

            GridIndex = 0;
            Grids = Enum.GetNames(typeof(GridType));

            ActionRules = typeof(ActionRules).Assembly.GetTypes()
                                                           .Where(t => t.IsClass && t.IsSubclassOf(typeof(ActionRules)))
                                                           .Select(t => t.Name)
                                                           .ToArray();
        }

        public void Draw(LevelWindow window)
        {
            if (string.IsNullOrEmpty(window.CachedRootFolderPath))
            {
                window.CachedRootFolderPath = window.Settings.DefaultItemFolder;
            }
            if (string.IsNullOrEmpty(window.CachedLevelName))
            {
                window.CachedLevelName = window.Settings.DefaultItemName;
            }

            var Border = window.Border;
            Common.x = 2 * Border;
            Common.y = 2 * Border;
            Common.width = window.WindowWidth - 4 * Border;
            Common.height = window.CommonHeight;
            EditorGUI.DrawRect(Common, window.CommonColor);
            CommonContent.x = Common.x + 2 * Border;
            CommonContent.y = Common.y + 2 * Border;
            CommonContent.width = Common.width - 4 * Border;
            CommonContent.height = Common.height - 4 * Border;

            GUILayout.BeginArea(CommonContent);
            window.Level = EditorGUILayout.ObjectField("Level Asset", window.Level, typeof(LevelData), allowSceneObjects: false) as LevelData;
            GUILayout.EndArea();

            WorkArea.x = Common.x;
            WorkArea.y = Common.y + Common.height + 2 * Border;
            WorkArea.width = window.WindowWidth - 4 * Border;
            WorkArea.height = window.WindowHeight - Common.y - Common.height - 10 * Border;
            EditorGUI.DrawRect(WorkArea, window.WorkAreaColor);
            WorkAreaContent.x = WorkArea.x + 2 * Border;
            WorkAreaContent.y = WorkArea.y + 2 * Border;
            WorkAreaContent.width = WorkArea.width - 4 * Border;
            WorkAreaContent.height = WorkArea.height - 4 * Border;

            GUILayout.BeginArea(WorkAreaContent);
            EditorGUIUtility.labelWidth = 70f;
            window.CachedRootFolderPath = EditorGUILayout.TextField("Folder Path", window.CachedRootFolderPath);
            window.CachedLevelName = EditorGUILayout.TextField("Filename", window.CachedLevelName);
            EditorGUIUtility.labelWidth = 90f;
            GridIndex = EditorGUILayout.Popup("Map Grid", GridIndex, Grids);
            AxisIndex = EditorGUILayout.Popup("Axis", AxisIndex, Axes);
            ActionIndex = EditorGUILayout.Popup("Action Rules", ActionIndex, ActionRules);
            ResolverIndex = EditorGUILayout.Popup("Turn Resolver", ResolverIndex, TurnResolvers);
            BattleFinishIndex = EditorGUILayout.Popup("Battle Finish:", BattleFinishIndex, BattleFinishers);

            if (GUILayout.Button("Create"))
            {
                window.Create(Grids[GridIndex], 
                              Axes[AxisIndex], 
                              ActionRules[ActionIndex], 
                              TurnResolvers[ResolverIndex], 
                              BattleFinishers[BattleFinishIndex]);
                window.Submenu = new Edit(window);
            }
            if (GUILayout.Button("Cancel"))
            {
                window.Submenu = new Edit(window);
            }
            GUILayout.EndArea();
        }
    }
}
