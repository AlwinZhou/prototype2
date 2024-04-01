using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Effect.Tabs
{
    public class Edit : IEffectHandlerEditor
    {
        public void Draw(EffectHandlerEditor tab, EffectWindow window)
        {
            if (tab.Info.Effect)
            {
                EditorGUIUtility.labelWidth = 130f;
                EditorGUILayout.LabelField(string.Format("Type: {0}", tab.Info.Effect.GetType().Name));
                var effectSerialized = tab.Info.Serialized;
                if (effectSerialized != null && effectSerialized.targetObject)
                {
                    if (GUILayout.Button("Change"))
                    {
                        window.UpdatePopUp();
                        tab.Menu = new Change();
                    }

                    GUILayout.Space(20f);
                    var prop = effectSerialized.GetIterator();
                    prop.NextVisible(true);
                    while (prop.NextVisible(true))
                    {
                        if (prop.depth == 0)
                        {
                            EditorGUILayout.PropertyField(prop, true);
                        }
                    }
                    effectSerialized.ApplyModifiedProperties();
                }
            }
            else
            {
                EditorGUILayout.LabelField("Type: None");
                if (GUILayout.Button("Create"))
                {
                    window.UpdatePopUp();
                    tab.Menu = new Create();
                }
            }
        }
    }
}
