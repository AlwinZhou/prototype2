using RedBjorn.SuperTiles.Editors.Effect.Tabs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Effect.Submenus
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

        public Edit(EffectWindow window)
        {
            Menus = new List<Tab>
            {
                new Tab { Caption =  "Visual", Submenu = new Visual(window)},
                new Tab { Caption =  nameof(EffectData.Stats), Submenu = new Stats()},
                new Tab { Caption =  nameof(EffectData.OnAdded), Submenu = new OnAdded(window)},
                new Tab { Caption =  nameof(EffectData.Handler), Submenu = new Handler(window)},
                new Tab { Caption =  nameof(EffectData.OnRemoved), Submenu = new OnRemoved(window)},
            };
        }

        public void Draw(EffectWindow window)
        {
            var Border = window.Border;
            //Common
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
            GUILayout.BeginHorizontal();
            window.Effect = EditorGUILayout.ObjectField("Effect Asset", window.Effect, typeof(EffectData), allowSceneObjects: false) as EffectData;
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            var enabled = GUI.enabled;
            GUI.enabled = window.Effect != null;
            if (GUILayout.Button("Duplicate", GUILayout.Height(25f)))
            {
                window.Submenu = new Duplicate();
                var path = AssetDatabase.GetAssetPath(window.Effect);
                window.CachedRootFolderPath = Path.GetDirectoryName(Path.GetDirectoryName(path));
                window.CachedEffectName = Path.GetFileNameWithoutExtension(path) + "New";
            }
            GUI.enabled = enabled;
            if (GUILayout.Button("Create", GUILayout.Height(25f)))
            {
                window.UpdatePopUp();
                window.EffectHandlerIndex = 1;
                window.Submenu = new Create();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            //Menu
            if (window.Effect)
            {
                Menu.x = Common.x;
                Menu.y = Common.y + Common.height + 2 * Border;
                Menu.width = window.MenuWidth;
                Menu.height = window.WindowHeight - Common.y - Common.height - 10 * Border;
            }
            else
            {
                Menu.x = 0f;
                Menu.y = 0f;
                Menu.width = 0f;
                Menu.height = 0f;
            }

            EditorGUI.DrawRect(Menu, window.MenuColor);
            MenuContent.x = Menu.x + 2 * Border;
            MenuContent.y = Menu.y + 2 * Border;
            MenuContent.width = Menu.width - 4 * Border;
            MenuContent.height = Menu.height - 4 * Border;

            GUILayout.BeginArea(MenuContent);
            window.Tab = GUILayout.SelectionGrid(window.Tab, Menus.Select(S => S.Caption).ToArray(), 1);
            GUILayout.EndArea();

            //Work
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
        }
    }
}