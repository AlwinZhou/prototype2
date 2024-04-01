using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Effect.Tabs
{
    public class Change : IEffectHandlerEditor
    {
        public void Draw(EffectHandlerEditor tab, EffectWindow window)
        {
            if (tab.Info == null || !tab.Info.Effect)
            {
                return;
            }
            EditorGUIUtility.labelWidth = 60f;
            window.EffectHandlerIndex = EditorGUILayout.Popup("Type: ", window.EffectHandlerIndex, window.EffectHandlers);

            var gui = GUI.enabled;
            var handlerType = window.EffectHandlers[window.EffectHandlerIndex];
            GUI.enabled = tab.Info.Effect.GetType().Name != handlerType;
            if (GUILayout.Button("Change"))
            {
                window.EffectHandlerChangeType(tab.Info, handlerType);
                tab.Menu = new Edit();
            }
            GUI.enabled = gui;
            if (GUILayout.Button("Cancel"))
            {
                tab.Menu = new Edit();
            }
        }
    }
}
