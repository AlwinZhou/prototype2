using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Level.Submenus.Tabs.Actions
{
    public class Change : IActionSubmenu
    {
        public int TurnType;
        public string[] Turns;

        public Change(LevelWindow levelWindow)
        {
            TurnType = 0;
            Turns = typeof(RedBjorn.SuperTiles.ActionRules).Assembly.GetTypes()
                        .Where(t => t.IsClass && t.IsSubclassOf(typeof(RedBjorn.SuperTiles.ActionRules)))
                        .Select(t => t.Name)
                        .ToArray();
        }

        public void Draw(ActionRules tab, LevelWindow levelWindow)
        {
            GUILayout.EndHorizontal();

            GUILayout.Space(20f);

            TurnType = EditorGUILayout.Popup("Type:", TurnType, Turns);

            var gui = GUI.enabled;
            var handlerType = Turns[TurnType];
            GUI.enabled = levelWindow.CachedTurn.GetType().Name != handlerType;
            if (GUILayout.Button("Change"))
            {
                levelWindow.ChangeActionRules(handlerType);
                tab.Submenu = new Edit(levelWindow);
            }
            GUI.enabled = gui;
            if (GUILayout.Button("Cancel"))
            {
                tab.Submenu = new Edit(levelWindow);
            }
        }
    }
}
