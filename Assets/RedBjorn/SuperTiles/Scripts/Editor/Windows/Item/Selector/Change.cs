using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Item.Selector
{
    public class Change : ISelectorSubmenu
    {
        public void Draw(ItemWindow window)
        {
            EditorGUIUtility.labelWidth = 60;
            window.TargetSelectorIndex = EditorGUILayout.Popup("Type:", window.TargetSelectorIndex, window.TargetSelectors);

            var gui = GUI.enabled;
            var selectorType = window.TargetSelectors[window.TargetSelectorIndex];
            GUI.enabled = window.CachedSelector.GetType().Name != selectorType;
            if (GUILayout.Button("Change"))
            {
                window.ItemChangeTargetSelector(selectorType);
                window.SelectorSubmenu = new Edit();
            }
            GUI.enabled = gui;
            if (GUILayout.Button("Cancel"))
            {
                window.SelectorSubmenu = new Edit();
            }
        }
    }
}
