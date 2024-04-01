using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.HealthRule.Submenus
{
    public class Duplicate : ISubmenu
    {
        Rect Common;
        Rect CommonContent;
        Rect WorkArea;
        Rect WorkAreaContent;

        public void Draw(HealthRuleWindow window)
        {
            EditorGUIUtility.labelWidth = 70f;
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
            window.Rule = EditorGUILayout.ObjectField("Rule Asset", window.Rule, typeof(ConvertRule), allowSceneObjects: false) as ConvertRule;
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
            window.CachedRuleName = EditorGUILayout.TextField("Rule Name", window.CachedRuleName);

            if (GUILayout.Button("Duplicate"))
            {
                window.RuleDuplicate();
                window.Submenu = new Edit();
            }
            if (GUILayout.Button("Cancel"))
            {
                window.Submenu = new Edit();
            }
            GUILayout.EndArea();
        }
    }
}
