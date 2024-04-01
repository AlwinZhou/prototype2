using RedBjorn.Utils;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedBjorn.SuperTiles.Editors.Unit.Submenus
{
    public class DuplicateSubmenu : ISubmenu
    {
        Rect Common;
        Rect CommonContent;
        Rect WorkArea;
        Rect WorkAreaContent;

        public DuplicateSubmenu(UnitWindow window)
        {
            var assetPath = AssetDatabase.GenerateUniqueAssetPath(AssetDatabase.GetAssetPath(window.Unit));
            window.CachedRootFolderPath = Path.GetDirectoryName(assetPath);
            window.CachedUnitName = Path.GetFileNameWithoutExtension(assetPath);
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
            EditorGUIUtility.labelWidth = 70f;
            window.CachedRootFolderPath = EditorGUILayout.TextField("Folder Path", window.CachedRootFolderPath);
            window.CachedUnitName = EditorGUILayout.TextField("Filename", window.CachedUnitName);
            if (GUILayout.Button("Duplicate"))
            {
                try
                {
                    if (!Directory.Exists(window.CachedRootFolderPath))
                    {
                        Directory.CreateDirectory(window.CachedRootFolderPath);
                    }

                    var path = string.Format("{0}/{1}{2}", window.CachedRootFolderPath, window.CachedUnitName, FileFormat.Asset);
                    var unitPath = AssetDatabase.GenerateUniqueAssetPath(path);
                    if (string.IsNullOrEmpty(unitPath))
                    {
                        throw new Exception($"Could not genereate unique path for {path}");
                    }

                    var unit = Object.Instantiate(window.Unit);
                    AssetDatabase.CreateAsset(unit, unitPath);

                    if (window.Unit.Avatar)
                    {
                        var avatarPath = AssetDatabase.GetAssetPath(window.Unit.Avatar);
                        var avatarPathNew = AssetDatabase.GenerateUniqueAssetPath(avatarPath);
                        if (string.IsNullOrEmpty(avatarPathNew))
                        {
                            throw new Exception($"Could not genereate unique path for {avatarPath}");
                        }
                        AssetDatabase.CopyAsset(avatarPath, avatarPathNew);
                        unit.Avatar = AssetDatabase.LoadAssetAtPath<Sprite>(avatarPathNew);
                    }

                    AssetDatabase.SaveAssets();
                    Log.I($"Unit was to duplicated to {unitPath}");
                    window.Unit = unit;
                }
                catch (Exception e)
                {
                    Log.E($"Unit duplication failed. {e.Message}");
                }
                finally
                {
                    AssetDatabase.Refresh();
                    window.Submenu = new EditSubmenu(window);
                }
            }
            if (GUILayout.Button("Cancel"))
            {
                window.Submenu = new EditSubmenu(window);
            }
            GUILayout.EndArea();
        }
    }
}