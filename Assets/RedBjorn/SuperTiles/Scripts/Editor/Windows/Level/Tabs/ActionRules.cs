using RedBjorn.SuperTiles.Editors.Level.Submenus.Tabs.Actions;
using System;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Level.Submenus.Tabs
{
    [Serializable]
    public class ActionRules : ITab
    {
        public IActionSubmenu Submenu;
        public Vector2 PlayersScroll;

        public ActionRules(LevelWindow window)
        {
            Submenu = new Level.Submenus.Tabs.Actions.Edit(window);
        }

        public void Draw(LevelWindow window)
        {
            PlayersScroll = EditorGUILayout.BeginScrollView(PlayersScroll);
            var common = window.Submenu.GetType() == typeof(Edit);
            if (window.Level && common)
            {
                GUILayout.BeginHorizontal();
                GUI.enabled = false;
                window.CachedActions = EditorGUILayout.ObjectField("Action Rules", window.CachedActions, typeof(SuperTiles.ActionRules), allowSceneObjects: false) as SuperTiles.ActionRules;
                GUI.enabled = true;
                Submenu.Draw(this, window);
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
