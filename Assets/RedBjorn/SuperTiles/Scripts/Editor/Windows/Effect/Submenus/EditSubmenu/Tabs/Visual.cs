using RedBjorn.SuperTiles.Editors.Effect.Submenus;
using System;
using UnityEditor;

namespace RedBjorn.SuperTiles.Editors.Effect.Tabs
{
    [Serializable]
    public class Visual : ITab
    {
        public Visual(EffectWindow window)
        {

        }

        public void Draw(EffectWindow window)
        {
            EditorGUIUtility.labelWidth = 120f;
            var serialized = window.SerializedObject;
            if (serialized != null && serialized.targetObject != null)
            {
                EditorGUILayout.PropertyField(serialized.FindProperty(nameof(EffectData.Caption)));
                EditorGUILayout.PropertyField(serialized.FindProperty(nameof(EffectData.Icon)));
                var fxAddShow = serialized.FindProperty(nameof(EffectData.FxAddShow));
                EditorGUILayout.PropertyField(fxAddShow);
                if (fxAddShow.boolValue)
                {
                    EditorGUILayout.PropertyField(serialized.FindProperty(nameof(EffectData.FxAddHandler)));
                }
                var fxRemoveShow = serialized.FindProperty(nameof(EffectData.FxRemoveShow));
                EditorGUILayout.PropertyField(fxRemoveShow);
                if (fxRemoveShow.boolValue)
                {
                    EditorGUILayout.PropertyField(serialized.FindProperty(nameof(EffectData.FxRemoveHandler)));
                }
            }
        }
    }
}
