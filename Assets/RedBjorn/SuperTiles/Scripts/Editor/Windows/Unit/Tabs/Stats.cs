using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Unit.Submenus.Edit.Tabs
{
    public class Stats : ITab
    {
        public Vector2 ScrollPos;

        public Stats(UnitWindow window)
        {

        }

        public void Draw(UnitWindow window)
        {
            ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);
            bool doBreak = false;
            var statsProperty = window.SerializedObject.FindProperty(nameof(UnitData.Stats));
            var emptyLabel = new GUIContent("");
            for (int i = 0; i < statsProperty.arraySize; i++)
            {
                var prop = statsProperty.GetArrayElementAtIndex(i);
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(prop.FindPropertyRelative(nameof(UnitData.StatData.Stat)), emptyLabel);
                EditorGUILayout.PropertyField(prop.FindPropertyRelative(nameof(UnitData.StatData.Value)), emptyLabel, GUILayout.Width(50f));

                if (GUILayout.Button("-", GUILayout.Width(30f)))
                {
                    doBreak = true;
                    statsProperty.DeleteArrayElementAtIndex(i);
                }
                GUILayout.EndHorizontal();

                if (doBreak)
                {
                    break;
                }
            }

            if (GUILayout.Button("+", GUILayout.Height(20f)))
            {
                statsProperty.arraySize++;
            }

            EditorGUILayout.EndScrollView();
        }
    }
}
