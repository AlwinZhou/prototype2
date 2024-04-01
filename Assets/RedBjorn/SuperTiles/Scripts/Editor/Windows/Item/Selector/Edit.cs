using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Item.Selector
{
    public class Edit : ISelectorSubmenu
    {
        public void Draw(ItemWindow window)
        {
            var type = "None";
            var gui = false;
            if (window.CachedSelector)
            {
                type = window.CachedSelector.GetType().Name;
                gui = true;
            }
            EditorGUILayout.LabelField($"Type: {type}");
            GUI.enabled = gui;
            if (GUILayout.Button("Change"))
            {
                window.UpdatePopUp();
                window.SelectorSubmenu = new Change();
            }
            GUI.enabled = true;
            if (window.SerializedSelector != null)
            {
                var prop = window.SerializedSelector.GetIterator();
                prop.NextVisible(true);
                while (prop.NextVisible(true))
                {
                    if (prop.depth == 0)
                    {
                        EditorGUILayout.PropertyField(prop, true);
                    }
                }
                window.SerializedSelector.ApplyModifiedProperties();
            }
        }
    }
}
