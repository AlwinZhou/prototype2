using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.HealthRule.Submenus.Tabs.Conditions
{
    public class Edit : IConditionSubmenu
    {
        public void Draw(Condition tab, HealthRuleWindow window)
        {
            var type = "None";
            var gui = false;
            if (window.CachedCondition)
            {
                gui = true;
                type = window.CachedCondition.GetType().Name;
            }
            EditorGUILayout.LabelField($"Type: {type}");
            GUI.enabled = gui;
            if (GUILayout.Button("Change"))
            {
                tab.ConditionSubmenu = new Change(window);
            }
            GUI.enabled = true;
            if (window.SerializedCondition != null)
            {
                var prop = window.SerializedCondition.GetIterator();
                prop.NextVisible(true);
                while (prop.NextVisible(true))
                {
                    if (prop.depth == 0)
                    {
                        EditorGUILayout.PropertyField(prop, true);
                    }
                }
                window.SerializedCondition.ApplyModifiedProperties();
            }
        }
    }
}
