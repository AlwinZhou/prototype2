using RedBjorn.Utils;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedBjorn.SuperTiles.Editors.Unit.Submenus
{
    public class CreateSubmenu : ISubmenu
    {
        Rect Common;
        Rect CommonContent;
        Rect WorkArea;
        Rect WorkAreaContent;

        public CreateSubmenu()
        {

        }

        public void Draw(UnitWindow window)
        {
            var border = window.Border;
            var commonHeight = window.CommonHeight;
            Common.x = 2 * border;
            Common.y = 2 * border;
            Common.width = window.WindowWidth - 4 * border;
            Common.height = commonHeight;
            EditorGUI.DrawRect(Common, window.Settings.CommonColor);
            CommonContent.x = Common.x + 2 * border;
            CommonContent.y = Common.y + 2 * border;
            CommonContent.width = Common.width - 4 * border;
            CommonContent.height = Common.height - 4 * border;

            GUILayout.BeginArea(CommonContent);
            window.Unit = EditorGUILayout.ObjectField("Unit Asset", window.Unit, typeof(UnitData), allowSceneObjects: false) as UnitData;
            GUILayout.EndArea();

            WorkArea.x = 2 * border;
            WorkArea.y = Common.y + Common.height + 2 * border;
            WorkArea.width = window.WindowWidth - 4 * border;
            WorkArea.height = window.WindowHeight - Common.y - Common.height - 10 * border;
            EditorGUI.DrawRect(WorkArea, window.Settings.WorkAreaColor);
            WorkAreaContent.x = WorkArea.x + 2 * border;
            WorkAreaContent.y = WorkArea.y + 2 * border;
            WorkAreaContent.width = WorkArea.width - 4 * border;
            WorkAreaContent.height = WorkArea.height - 4 * border;

            GUILayout.BeginArea(WorkAreaContent);
            if (string.IsNullOrEmpty(window.CachedRootFolderPath))
            {
                window.CachedRootFolderPath = window.Settings.DefaultItemFolder;
            }
            if (string.IsNullOrEmpty(window.CachedUnitName))
            {
                window.CachedUnitName = window.Settings.DefaultItemName;
            }

            EditorGUIUtility.labelWidth = 70f;
            window.CachedRootFolderPath = EditorGUILayout.TextField("Folder Path", window.CachedRootFolderPath);
            window.CachedUnitName = EditorGUILayout.TextField("Filename", window.CachedUnitName);
            EditorGUIUtility.labelWidth = 90f;

            if (GUILayout.Button("Create"))
            {
                var unit = UnitData.Create(window.CachedRootFolderPath, window.CachedUnitName, S.Battle.Tags.Unit.GetDefault());
                if (unit)
                {
                    window.Unit = unit;
                }
                window.Submenu = new EditSubmenu(window);
            }
            if (GUILayout.Button("Cancel"))
            {
                window.Submenu = new EditSubmenu(window);
            }
            GUILayout.EndArea();
        }
    }
}