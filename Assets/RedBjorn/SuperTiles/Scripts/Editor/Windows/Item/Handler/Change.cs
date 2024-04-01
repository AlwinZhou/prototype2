using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Item.Handler
{
    public class Change : IHandlerSubmenu
    {
        public void Draw(ItemWindow window)
        {
            EditorGUIUtility.labelWidth = 60f;
            window.ActionHandlerIndex = EditorGUILayout.Popup("Type: ", window.ActionHandlerIndex, window.ActionHandlers);

            var gui = GUI.enabled;
            var handlerType = window.ActionHandlers[window.ActionHandlerIndex];
            GUI.enabled = window.CachedHandler.GetType().Name != handlerType;
            if (GUILayout.Button("Change"))
            {
                window.ItemChangeActionHandler(handlerType);
                window.HandlerSubmenu = new Edit();

            }
            GUI.enabled = gui;
            if (GUILayout.Button("Cancel"))
            {
                window.HandlerSubmenu = new Edit();
            }
        }
    }
}
