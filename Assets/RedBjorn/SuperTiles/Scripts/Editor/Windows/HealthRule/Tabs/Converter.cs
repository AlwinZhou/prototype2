using RedBjorn.SuperTiles.Health;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.HealthRule.Submenus.Tabs
{
    public class Converter : ITab
    {
        public void Draw(HealthRuleWindow window)
        {
            if (window.Rule)
            {
                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                var gui = GUI.enabled;
                GUI.enabled = false;
                var labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 90f;
                window.CachedConverter = EditorGUILayout.ObjectField("Converter", window.CachedConverter, typeof(ValueConverter), allowSceneObjects: false) as ValueConverter;
                EditorGUIUtility.labelWidth = labelWidth;
                GUI.enabled = gui;
                GUILayout.EndHorizontal();
                GUILayout.Space(20f);
                window.ConverterSubmenu.Draw(window);

                GUILayout.EndVertical();
            }
        }
    }
}
