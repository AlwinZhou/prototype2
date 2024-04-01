using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.HealthRule.Submenus.Tabs.Conditions
{
    public class Change : IConditionSubmenu
    {
        public Change(HealthRuleWindow window)
        {
            window.UpdatePopUp();
        }

        public void Draw(Condition tab, HealthRuleWindow window)
        {
            EditorGUIUtility.labelWidth = 60;
            window.ConditionIndex = EditorGUILayout.Popup("Type:", window.ConditionIndex, window.Conditions);

            var gui = GUI.enabled;
            var selectorType = window.Conditions[window.ConditionIndex];
            GUI.enabled = window.CachedCondition.GetType().Name != selectorType;
            if (GUILayout.Button("Change"))
            {
                window.RuleChangeCondition(selectorType);
                tab.ConditionSubmenu = new Edit();
            }
            GUI.enabled = gui;
            if (GUILayout.Button("Cancel"))
            {
                tab.ConditionSubmenu = new Edit();
            }
        }
    }
}
