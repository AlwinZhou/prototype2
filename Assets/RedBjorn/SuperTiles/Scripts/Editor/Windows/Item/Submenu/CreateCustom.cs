using System.IO;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Item
{
    public class CreateCustom : ISubmenu
    {
        Rect Common;
        Rect CommonContent;
        Rect WorkArea;
        Rect WorkAreaContent;

        public void Draw(ItemWindow window)
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
            window.Item = EditorGUILayout.ObjectField("Item Asset", window.Item, typeof(ItemData), allowSceneObjects: false) as ItemData;
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            var enabled = GUI.enabled;
            GUI.enabled = window.Item != null;
            if (GUILayout.Button("Duplicate", GUILayout.Height(25f)))
            {
                window.Submenu = new Duplicate();
                var path = AssetDatabase.GetAssetPath(window.Item);
                window.CachedRootFolderPath = Path.GetDirectoryName(Path.GetDirectoryName(path));
                window.CachedItemName = Path.GetFileNameWithoutExtension(path) + "New";
            }
            GUI.enabled = enabled;
            if (GUILayout.Button("Create", GUILayout.Height(25f)))
            {
                window.Submenu = new CreateCommon();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            //Work
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
            window.CachedRootFolderPath = EditorGUILayout.TextField("Path", window.CachedRootFolderPath);
            window.CachedItemName = EditorGUILayout.TextField("Item Name", window.CachedItemName);

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            window.TargetSelectorIndex = EditorGUILayout.Popup(window.TargetSelectorIndex, window.TargetSelectors);
            window.ActionHandlerIndex = EditorGUILayout.Popup(window.ActionHandlerIndex, window.ActionHandlers);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Create", GUILayout.Height(25f)))
            {
                window.ItemCreateNew(window.TargetSelectors[window.TargetSelectorIndex], window.ActionHandlers[window.ActionHandlerIndex]);
                window.Submenu = new Edit(window);
            }
            if (GUILayout.Button("Cancel", GUILayout.Height(25f)))
            {
                window.Submenu = new CreateCommon();
            }
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }
}
