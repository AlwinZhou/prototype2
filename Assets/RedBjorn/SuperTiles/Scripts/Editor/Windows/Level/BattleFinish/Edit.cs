using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Level.Submenus.Tabs.BattleFinish
{
    public class Edit : IBattleFinishSubmenu
    {
        public void Draw(BattleFinishTab tab, LevelWindow window)
        {
            var gui = GUI.enabled;
            var serialized = window.SerializedBattleFinish;

            GUILayout.EndHorizontal();

            GUILayout.Space(20f);

            if (window.CachedBattleFinish)
            {
                EditorGUILayout.LabelField(string.Format("Type: {0}", window.CachedBattleFinish.GetType().Name));
                if (GUILayout.Button("Change"))
                {
                    tab.Submenu = new BattleFinish.Change();
                }
            }

            if (serialized != null)
            {
                var prop = serialized.GetIterator();
                prop.NextVisible(true);
                while (prop.NextVisible(true))
                {
                    if (prop.depth == 0)
                    {
                        EditorGUILayout.PropertyField(prop, true);
                    }
                }
                serialized.ApplyModifiedProperties();
            }
            GUI.enabled = gui;
        }
    }
}
