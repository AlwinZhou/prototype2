using System.IO;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Level.Submenus
{
    public class Duplicate : ISubmenu
    {
        public Rect Common;
        public Rect CommonContent;
        public Rect WorkArea;
        public Rect WorkAreaContent;

        public Duplicate(LevelWindow window)
        {
            var path = AssetDatabase.GetAssetPath(window.Level);
            var folder = Path.GetDirectoryName(path);
            folder = AssetDatabase.GenerateUniqueAssetPath(folder);
            var filename = Path.GetFileName(folder);
            window.CachedRootFolderPath = folder;
            window.CachedLevelName = filename;
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
            if (GUILayout.Button("Duplicate"))
            {
                window.Duplicate();
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
