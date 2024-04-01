using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Item.Tabs
{
    public class Stats : ITab
    {
        public Stats(ItemWindow window)
        {

        }

        public void Draw(ItemWindow window)
        {
            if (window.SerializedObject != null && window.SerializedObject.targetObject != null)
            {
                window.ScrollPos = EditorGUILayout.BeginScrollView(window.ScrollPos);
                GUILayout.BeginHorizontal(GUILayout.Height(20f));
                var labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 80f;
                var stackableProp = window.SerializedObject.FindProperty(nameof(ItemData.Stackable));
                EditorGUILayout.PropertyField(stackableProp, GUILayout.Width(120f));
                if (stackableProp.boolValue)
                {
                    EditorGUIUtility.labelWidth = 100f;
                    EditorGUILayout.PropertyField(window.SerializedObject.FindProperty(nameof(ItemData.MaxStackCount)), GUILayout.Width(160f));
                }
                EditorGUIUtility.labelWidth = labelWidth;
                GUILayout.EndHorizontal();
                EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);
                var statsProperty = window.SerializedObject.FindProperty(nameof(window.Item.Stats));
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

}
