
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Level.Submenus
{
    public class Edit : ISubmenu
    {
        public Rect Common;
        public Rect CommonContent;
        public Rect Menu;
        public Rect MenuContent;
        public Rect WorkArea;
        public Rect WorkAreaContent;
        public List<Tab> Menus;

        public Edit(LevelWindow window)
        {
            Menus = new List<Tab>();
            Menus.Add(new Tab { Caption = nameof(LevelData.Map), Submenu = new Tabs.Map(window) });
            Menus.Add(new Tab { Caption = nameof(LevelData.Turn), Submenu = new Tabs.TurnResolver(window) });
            Menus.Add(new Tab { Caption = "Action Rules", Submenu = new Tabs.ActionRules(window) });
            Menus.Add(new Tab { Caption = "Battle Actions", Submenu = new Tabs.BattleActions(window) });
            Menus.Add(new Tab { Caption = "Finish Conditions", Submenu = new Tabs.BattleFinishTab(window) });
            Menus.Add(new Tab { Caption = nameof(LevelData.Health), Submenu = new Tabs.Health(window) });
            Menus.Add(new Tab { Caption = nameof(LevelData.Players), Submenu = new Tabs.Players(window) });
            Menus.Add(new Tab { Caption = nameof(LevelData.Camera), Submenu = new Tabs.CameraTab(window) });
            Menus.Add(new Tab { Caption = nameof(LevelData.Relationship), Submenu = new Tabs.RelationshipTab(window) });
            Menus.Add(new Tab { Caption = "Other", Submenu = new Tabs.OtherTab(window) });
        }

        public void Draw(LevelWindow window)
        {
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
            var so = window.SerializedObject;
            var gui = GUI.enabled;
            GUI.enabled = window.Level;
            if (so != null && so.targetObject != null)
            {
                var prop = so.FindProperty(nameof(LevelData.Caption));
                EditorGUILayout.PropertyField(prop);
                prop = window.SerializedObject.FindProperty(nameof(LevelData.SceneName));
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(prop);
                if (GUILayout.Button("Active Scene", GUILayout.Width(100f)))
                {
                    prop.stringValue = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                    window.Dirty = true;
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUI.enabled = window.Level;
            if (GUILayout.Button("Duplicate", GUILayout.Height(window.ButtonHeight)))
            {
                window.Submenu = new Duplicate(window);
            }
            GUI.enabled = gui;
            if (GUILayout.Button("Create", GUILayout.Height(window.ButtonHeight)))
            {
                window.Submenu = new Create();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            float x = 0;
            float y = 0;
            float menuWidth = 0;
            float menuheight = 0;
            var common = window.Submenu.GetType() == typeof(Edit);
            if (window.Level)
            {
                if (common)
                {
                    x = Common.x;
                    y = Common.y + Common.height + 2 * Border;
                    menuWidth = window.MenuWidth;
                    menuheight = window.WindowHeight - Common.y - Common.height - 10 * Border;
                }
            }
            Menu.x = x;
            Menu.y = y;
            Menu.width = menuWidth;
            Menu.height = menuheight;
            EditorGUI.DrawRect(Menu, window.MenuColor);
            MenuContent.x = Menu.x + 2 * Border;
            MenuContent.y = Menu.y + 2 * Border;
            MenuContent.width = Menu.width - 4 * Border;
            MenuContent.height = Menu.height - 4 * Border;

            GUILayout.BeginArea(MenuContent);
            window.Tab = GUILayout.SelectionGrid(window.Tab, Menus.Select(S => S.Caption).ToArray(), 1);
            GUILayout.EndArea();

            WorkArea.x = Menu.x + Menu.width + 2 * Border;
            WorkArea.y = Common.y + Common.height + 2 * Border;
            WorkArea.width = window.WindowWidth - Menu.width - Menu.x - 4 * Border;
            WorkArea.height = window.WindowHeight - Common.y - Common.height - 10 * Border;
            EditorGUI.DrawRect(WorkArea, window.WorkAreaColor);
            WorkAreaContent.x = WorkArea.x + 2 * Border;
            WorkAreaContent.y = WorkArea.y + 2 * Border;
            WorkAreaContent.width = WorkArea.width - 4 * Border;
            WorkAreaContent.height = WorkArea.height - 4 * Border;

            GUILayout.BeginArea(WorkAreaContent);
            Menus[window.Tab].Submenu.Draw(window);
            GUILayout.EndArea();
            GUI.enabled = gui;
        }
    }
}