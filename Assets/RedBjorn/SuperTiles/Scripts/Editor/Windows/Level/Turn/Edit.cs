using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Level.Submenus.Tabs.Turn
{
    public class Edit : ITurnSubmenu
    {
        public void Draw(TurnResolver tab, LevelWindow levelWindow)
        {
            var gui = GUI.enabled;
            var serializedTurn = levelWindow.SerializedTurn;

            GUILayout.EndHorizontal();

            GUILayout.Space(20f);

            if (levelWindow.CachedTurn)
            {
                EditorGUILayout.LabelField(string.Format("Type: {0}", levelWindow.CachedTurn.GetType().Name));
                if (GUILayout.Button("Change"))
                {
                    tab.TurnSubmenu = new Level.Submenus.Tabs.Turn.Change();
                }
            }

            if (serializedTurn != null)
            {
                var prop = serializedTurn.GetIterator();
                prop.NextVisible(true);
                while (prop.NextVisible(true))
                {
                    if (prop.depth == 0)
                    {
                        EditorGUILayout.PropertyField(prop, true);
                    }
                }
                serializedTurn.ApplyModifiedProperties();
            }
            GUI.enabled = gui;
        }
    }
}
