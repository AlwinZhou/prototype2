using RedBjorn.SuperTiles.Editors.Effect.Submenus;
using System;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Effect.Tabs
{
    [Serializable]
    public class Stats : ITab
    {
        public void Draw(EffectWindow window)
        {
            if (window.SerializedObject == null)
            {
                return;
            }
            window.ScrollPos = EditorGUILayout.BeginScrollView(window.ScrollPos);
            GUILayout.BeginHorizontal(GUILayout.Height(20f));
            var labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 80f;
            EditorGUIUtility.labelWidth = labelWidth;
            GUILayout.EndHorizontal();
            EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);
            var statsProperty = window.SerializedObject.FindProperty(nameof(window.Effect.Stats));
            for (int j = 0; j < statsProperty.arraySize; j++)
            {
                GUILayout.BeginHorizontal(GUILayout.Height(20f));
                var stat = statsProperty.GetArrayElementAtIndex(j);
                var statProperty = stat.FindPropertyRelative("Stat");
                var valueProperty = stat.FindPropertyRelative("Value");
                EditorGUILayout.PropertyField(statProperty, new GUIContent(""), true);
                EditorGUILayout.PropertyField(valueProperty, new GUIContent(""), true, GUILayout.Width(50f));
                if (GUILayout.Button("-", GUILayout.Width(20f)))
                {
                    stat.DeleteCommand();
                    GUILayout.EndHorizontal();
                    break;
                }
                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("+", GUILayout.Height(20)))
            {
                statsProperty.arraySize++;
                var stat = statsProperty.GetArrayElementAtIndex(statsProperty.arraySize - 1);
                var statProperty = stat.FindPropertyRelative("Stat");
                statProperty.objectReferenceValue = null;
                var valueProperty = stat.FindPropertyRelative("Value");
                valueProperty.floatValue = 100f;
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
