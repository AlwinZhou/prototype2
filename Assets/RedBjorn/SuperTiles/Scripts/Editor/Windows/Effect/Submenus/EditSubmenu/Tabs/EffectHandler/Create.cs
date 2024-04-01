using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Effect.Tabs
{
    public class Create : IEffectHandlerEditor
    {
        public void Draw(EffectHandlerEditor tab, EffectWindow window)
        {
            if (tab.Info == null)
            {
                return;
            }
            EditorGUIUtility.labelWidth = 60f;
            window.EffectHandlerIndex = EditorGUILayout.Popup("Type: ", window.EffectHandlerIndex, window.EffectHandlers);

            if (GUILayout.Button("Create"))
            {
                window.EffectHandlerChangeType(tab.Info, window.EffectHandlers[window.EffectHandlerIndex], tab.Suffix);
                tab.Menu = new Edit();
            }

            if (GUILayout.Button("Cancel"))
            {
                tab.Menu = new Edit();
            }
        }
    }
}
