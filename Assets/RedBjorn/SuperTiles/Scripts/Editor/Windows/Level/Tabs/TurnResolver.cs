using RedBjorn.SuperTiles.Editors.Level.Submenus.Tabs.Turn;
using System;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Level.Submenus.Tabs
{
    [Serializable]
    public class TurnResolver : ITab
    {
        public Vector2 PlayersScroll;
        public ITurnSubmenu TurnSubmenu;

        public TurnResolver(LevelWindow window)
        {
            TurnSubmenu = new Level.Submenus.Tabs.Turn.Edit();
        }

        public void Draw(LevelWindow window)
        {
            PlayersScroll = EditorGUILayout.BeginScrollView(PlayersScroll);
            var common = window.Submenu.GetType() == typeof(Edit);
            if (window.Level && common)
            {
                GUILayout.BeginHorizontal();
                GUI.enabled = false;
                window.CachedTurn = EditorGUILayout.ObjectField("Turn Resolver", window.CachedTurn, typeof(SuperTiles.TurnResolver), allowSceneObjects: false) as SuperTiles.TurnResolver;
                GUI.enabled = true;
                TurnSubmenu.Draw(this, window);
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
