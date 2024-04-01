using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.HealthRule.Submenus.Tabs.Converters
{
    public class Edit : IConverterSubmenu
    {
        public void Draw(HealthRuleWindow window)
        {
            var type = "None";
            var gui = false;
            if (window.CachedConverter)
            {
                gui = true;
                type = window.CachedConverter.GetType().Name;
            }
            EditorGUILayout.LabelField($"Type: {type}");
            GUI.enabled = gui;
            if (GUILayout.Button("Change"))
            {
                window.ConverterSubmenu = new Change(window);
            }
            GUI.enabled = true;
            if (window.SerializedConverter != null)
            {
                GUILayout.Space(20f);
                var prop = window.SerializedConverter.GetIterator();
                prop.NextVisible(true);
                while (prop.NextVisible(true))
                {
                    if (prop.depth == 0)
                    {
                        EditorGUILayout.PropertyField(prop, true);
                    }
                }
                window.SerializedConverter.ApplyModifiedProperties();
            }
        }
    }
}
