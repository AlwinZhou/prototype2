using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Item.Handler
{
    public class Edit : IHandlerSubmenu
    {
        public void Draw(ItemWindow window)
        {
            var type = "None";
            var gui = false;
            if (window.CachedHandler)
            {
                type = window.CachedHandler.GetType().Name;
                gui = true;
            }
            EditorGUILayout.LabelField($"Type: {type}");
            GUI.enabled = gui;
            if (GUILayout.Button("Change"))
            {
                window.UpdatePopUp();
                window.HandlerSubmenu = new Change();
            }

            GUI.enabled = true;
            if (window.SerializedHandler != null)
            {
                GUILayout.Space(20f);
                var prop = window.SerializedHandler.GetIterator();
                prop.NextVisible(true);
                while (prop.NextVisible(true))
                {
                    if (prop.depth == 0)
                    {
                        EditorGUILayout.PropertyField(prop, true);
                    }
                }
                window.SerializedHandler.ApplyModifiedProperties();
            }
        }
    }
}
