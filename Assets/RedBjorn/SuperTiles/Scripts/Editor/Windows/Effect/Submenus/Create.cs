using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Effect.Submenus
{
    public class Create : ISubmenu
    {
        public Rect Common;
        public Rect CommonContent;
        public Rect WorkArea;
        public Rect WorkAreaContent;

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
            GUILayout.EndArea();


            //Work
            WorkArea.x = Common.x + 2 * Border;
            WorkArea.y = Common.y + Common.height + 2 * Border;
            WorkArea.width = window.WindowWidth - Common.x - 4 * Border;
            WorkArea.height = window.WindowHeight - Common.y - Common.height - 10 * Border;
            EditorGUI.DrawRect(WorkArea, window.WorkAreaColor);
            WorkAreaContent.x = WorkArea.x + 2 * Border;
            WorkAreaContent.y = WorkArea.y + 2 * Border;
            WorkAreaContent.width = WorkArea.width - 4 * Border;
            WorkAreaContent.height = WorkArea.height - 4 * Border;

            GUILayout.BeginArea(WorkAreaContent);
            if (string.IsNullOrEmpty(window.CachedRootFolderPath))
            {
                window.CachedRootFolderPath = window.Settings.DefaultEffectFolder;
            }
            window.CachedRootFolderPath = EditorGUILayout.TextField("Path", window.CachedRootFolderPath);
            window.CachedEffectName = EditorGUILayout.TextField("Effect Name", window.CachedEffectName);
            GUILayout.BeginHorizontal();
            var height = GUILayout.Height(50f);
            GUILayout.BeginVertical();
            GUILayout.BeginVertical();
            window.EffectOnAddedIndex = EditorGUILayout.Popup("On Added", window.EffectOnAddedIndex, window.EffectHandlers);
            window.EffectHandlerIndex = EditorGUILayout.Popup("Handler", window.EffectHandlerIndex, window.EffectHandlers);
            window.EffectOnRemovedIndex = EditorGUILayout.Popup("On Removed", window.EffectOnRemovedIndex, window.EffectHandlers);
            if (GUILayout.Button("Create", GUILayout.Height(25f)))
            {
                window.EffectCreateNew(window.EffectHandlers[window.EffectOnAddedIndex],
                                       window.EffectHandlers[window.EffectHandlerIndex],
                                       window.EffectHandlers[window.EffectOnRemovedIndex]);
                window.Submenu = new Edit(window);
            }
            if (GUILayout.Button("Cancel", GUILayout.Height(25f)))
            {
                window.Submenu = new Edit(window);
            }
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }
}
