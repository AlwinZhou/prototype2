using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Level.Submenus.Tabs.BattleFinish
{
    public class Change : IBattleFinishSubmenu
    {
        public int Type;
        public string[] BattleFinishers;

        public Change()
        {
            Type = 0;
            BattleFinishers = typeof(RedBjorn.SuperTiles.BattleFinishHandler).Assembly.GetTypes()
                        .Where(t => t.IsClass && t.IsSubclassOf(typeof(RedBjorn.SuperTiles.BattleFinishHandler)))
                        .Select(t => t.Name)
                        .ToArray();
        }

        public void Draw(BattleFinishTab tab, LevelWindow window)
        {
            GUILayout.EndHorizontal();

            GUILayout.Space(20f);

            Type = EditorGUILayout.Popup("Type", Type, BattleFinishers);

            var gui = GUI.enabled;
            var handlerType = BattleFinishers[Type];
            GUI.enabled = window.CachedBattleFinish.GetType().Name != handlerType;
            if (GUILayout.Button("Change"))
            {
                window.ChangeBattleFinish(handlerType);
                tab.Submenu = new Edit();
            }
            GUI.enabled = gui;
            if (GUILayout.Button("Cancel"))
            {
                tab.Submenu = new Edit();
            }
        }
    }
}