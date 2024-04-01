using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Unit.Submenus
{
    public class EditSubmenu : ISubmenu
    {
        Rect Common;
        Rect CommonContent;
        Rect Menu;
        Rect MenuContent;
        Rect WorkArea;
        Rect WorkAreaContent;
        public List<Edit.Tab> Tabs;

        public EditSubmenu(UnitWindow window)
        {
            Tabs = new List<Edit.Tab>();
            Tabs.Add(new Edit.Tab { Caption = "Visual", Drawer = new Edit.Tabs.Visual(window) });
            Tabs.Add(new Edit.Tab { Caption = "Tags", Drawer = new Edit.Tabs.Tags(window) });
            Tabs.Add(new Edit.Tab { Caption = "Stats", Drawer = new Edit.Tabs.Stats(window) });
            Tabs.Add(new Edit.Tab { Caption = "Default Items", Drawer = new Edit.Tabs.DefaultItems(window) });
            Tabs.Add(new Edit.Tab { Caption = "Actions", Drawer = new Edit.Tabs.Actions(window) });
        }

        public void Draw(UnitWindow window)
        {
            var Border = window.Border;
            var MenuWidth = window.MenuWidth;
            var commonHeight = window.CommonHeight;
            Common.x = 2 * Border;
            Common.y = 2 * Border;
            Common.width = window.WindowWidth - 4 * Border;
            Common.height = commonHeight;
            EditorGUI.DrawRect(Common, window.Settings.CommonColor);
            CommonContent.x = Common.x + 2 * Border;
            CommonContent.y = Common.y + 2 * Border;
            CommonContent.width = Common.width - 4 * Border;
            CommonContent.height = Common.height - 4 * Border;

            GUILayout.BeginArea(CommonContent);
            window.Unit = EditorGUILayout.ObjectField("Unit Asset", window.Unit, typeof(UnitData), allowSceneObjects: false) as UnitData;
            GUILayout.BeginHorizontal();
            GUI.enabled = window.Unit;
            if (GUILayout.Button("Duplicate", GUILayout.Height(25f)))
            {
                window.Submenu = new DuplicateSubmenu(window);
            }
            GUI.enabled = true;
            if (GUILayout.Button("Create", GUILayout.Height(25f)))
            {
                window.Submenu = new CreateSubmenu();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            float x = 0;
            float y = 0;
            float menuWidth = 0;
            float menuheight = 0;
            if (window.Unit)
            {
                x = Common.x;
                y = Common.y + Common.height + 2 * Border;
                menuWidth = MenuWidth;
                menuheight = window.WindowHeight - Common.y - Common.height - 10 * Border;
            }
            Menu.x = x;
            Menu.y = y;
            Menu.width = menuWidth;
            Menu.height = menuheight;
            EditorGUI.DrawRect(Menu, window.Settings.MenuColor);
            MenuContent.x = Menu.x + 2 * Border;
            MenuContent.y = Menu.y + 2 * Border;
            MenuContent.width = Menu.width - 4 * Border;
            MenuContent.height = Menu.height - 4 * Border;

            GUILayout.BeginArea(MenuContent);
            window.Tab = GUILayout.SelectionGrid(window.Tab, Tabs.Select(m => m.Caption).ToArray(), 1);
            GUILayout.EndArea();

            WorkArea.x = Menu.x + Menu.width + 2 * Border;
            WorkArea.y = Common.y + Common.height + 2 * Border;
            WorkArea.width = window.WindowWidth - Menu.width - Menu.x - 4 * Border;
            WorkArea.height = window.WindowHeight - Common.y - Common.height - 10 * Border;
            EditorGUI.DrawRect(WorkArea, window.Settings.WorkAreaColor);
            WorkAreaContent.x = WorkArea.x + 2 * Border;
            WorkAreaContent.y = WorkArea.y + 2 * Border;
            WorkAreaContent.width = WorkArea.width - 4 * Border;
            WorkAreaContent.height = WorkArea.height - 4 * Border;

            GUILayout.BeginArea(WorkAreaContent);
            if (window.Unit)
            {
                EditorGUIUtility.labelWidth = window.WindowWidth / 4f;
                Tabs[window.Tab].Drawer.Draw(window);
            }
            GUILayout.EndArea();
        }
    }
}