using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Level.Submenus.Tabs.Turn
{
    public class Change : ITurnSubmenu
    {
        public int TurnType;
        public string[] Turns;

        public Change()
        {
            TurnType = 0;
            Turns = typeof(RedBjorn.SuperTiles.TurnResolver).Assembly.GetTypes()
                        .Where(t => t.IsClass && t.IsSubclassOf(typeof(RedBjorn.SuperTiles.TurnResolver)))
                        .Select(t => t.Name)
                        .ToArray();
        }

        public void Draw(TurnResolver tab, LevelWindow levelWindow)
        {
            GUILayout.EndHorizontal();

            GUILayout.Space(20f);

            TurnType = EditorGUILayout.Popup("Type", TurnType, Turns);

            var gui = GUI.enabled;
            var handlerType = Turns[TurnType];
            GUI.enabled = levelWindow.CachedTurn.GetType().Name != handlerType;
            if (GUILayout.Button("Change"))
            {
                levelWindow.ChangeTurnResolver(handlerType);
                tab.TurnSubmenu = new Edit();
            }
            GUI.enabled = gui;
            if (GUILayout.Button("Cancel"))
            {
                tab.TurnSubmenu = new Edit();
            }
        }
    }
}