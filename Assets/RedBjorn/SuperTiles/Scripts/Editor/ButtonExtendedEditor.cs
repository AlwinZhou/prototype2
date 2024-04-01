using RedBjorn.SuperTiles.UI;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    [CustomEditor(typeof(ButtonExtended))]
    public class ButtonExtendedEditor : ButtonEditor
    {

        SerializedProperty AudioClip;

        protected override void OnEnable()
        {
            base.OnEnable();
            AudioClip = serializedObject.FindProperty("Clip");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var button = (ButtonExtended)target;
            button.CommonSound = EditorGUILayout.Toggle("Common Sound", button.CommonSound, GUILayout.Height(16f));
            if (!button.CommonSound)
            {
                EditorGUILayout.PropertyField(AudioClip, new GUIContent("Audio Clip"), GUILayout.Height(16f));
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}