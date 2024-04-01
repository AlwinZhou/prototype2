using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.HealthRule.Submenus.Tabs.Converters
{
    public class Change : IConverterSubmenu
    {
        public Change(HealthRuleWindow window)
        {
            window.UpdatePopUp();
        }

        public void Draw(HealthRuleWindow window)
        {
            EditorGUIUtility.labelWidth = 60f;
            window.ConverterIndex = EditorGUILayout.Popup("Type: ", window.ConverterIndex, window.Converters);

            var gui = GUI.enabled;
            var handlerType = window.Converters[window.ConverterIndex];
            GUI.enabled = window.CachedConverter.GetType().Name != handlerType;
            if (GUILayout.Button("Change"))
            {
                window.RuleChangeConverter(handlerType);
                window.ConverterSubmenu = new Edit();
            }
            GUI.enabled = gui;
            if (GUILayout.Button("Cancel"))
            {
                window.ConverterSubmenu = new Edit();
            }
        }
    }
}
