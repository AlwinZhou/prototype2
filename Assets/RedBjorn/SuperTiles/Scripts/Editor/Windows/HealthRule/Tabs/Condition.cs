using RedBjorn.SuperTiles.Editors.HealthRule.Submenus.Tabs.Conditions;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.HealthRule.Submenus.Tabs
{
    public class Condition : ITab
    {
        public IConditionSubmenu ConditionSubmenu;

        public Condition()
        {
            ConditionSubmenu = new Conditions.Edit();
        }
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
                window.CachedCondition = EditorGUILayout.ObjectField("Condition", window.CachedCondition, typeof(Health.Condition), allowSceneObjects: false) as Health.Condition;
                EditorGUIUtility.labelWidth = labelWidth;
                GUI.enabled = gui;
                GUILayout.EndHorizontal();
                GUILayout.Space(20f);
                ConditionSubmenu.Draw(this, window);
                GUILayout.EndVertical();
            }
        }
    }
}
