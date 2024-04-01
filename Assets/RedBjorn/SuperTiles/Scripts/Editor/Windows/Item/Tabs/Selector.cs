using RedBjorn.SuperTiles.Items;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Item.Tabs
{
    public class Selector : ITab
    {
        public Selector(ItemWindow window)
        {

        }

        public void Draw(ItemWindow window)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            var gui = GUI.enabled;
            GUI.enabled = false;
            var labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 90f;
            window.CachedSelector = EditorGUILayout.ObjectField("Target Selector", window.CachedSelector, typeof(TargetSelector), allowSceneObjects: false) as TargetSelector;
            EditorGUIUtility.labelWidth = labelWidth;
            GUI.enabled = gui;
            GUILayout.EndHorizontal();
            GUILayout.Space(20f);
            window.SelectorSubmenu.Draw(window);
            GUILayout.EndVertical();
        }
    }

}
