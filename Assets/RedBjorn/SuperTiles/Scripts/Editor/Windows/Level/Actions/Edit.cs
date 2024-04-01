using UnityEditor;
using UnityEngine;


namespace RedBjorn.SuperTiles.Editors.Level.Submenus.Tabs.Actions
{
    public class Edit : IActionSubmenu
    {
        public Edit(LevelWindow levelWindow)
        {

        }

        public void Draw(ActionRules tab, LevelWindow levelWindow)
        {
            GUILayout.EndHorizontal();
            var gui = GUI.enabled;
            EditorGUIUtility.labelWidth = 150f;
            GUILayout.Space(20f);
            if (levelWindow.CachedActions)
            {
                EditorGUILayout.LabelField(string.Format("Type: {0}", levelWindow.CachedActions.GetType().Name));
                if (GUILayout.Button("Change"))
                {
                    tab.Submenu = new Change(levelWindow);
                }
            }

            var serializedActions = levelWindow.SerializedActions;
            if (serializedActions != null)
            {
                var prop = serializedActions.GetIterator();
                prop.NextVisible(true);
                while (prop.NextVisible(true))
                {
                    if (prop.depth == 0)
                    {
                        EditorGUILayout.PropertyField(prop, true);
                    }
                }
                serializedActions.ApplyModifiedProperties();
            }
            GUI.enabled = gui;
        }
    }
}
