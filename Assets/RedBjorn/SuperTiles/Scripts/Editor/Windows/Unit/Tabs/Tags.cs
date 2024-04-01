using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Unit.Submenus.Edit.Tabs
{
    public class Tags : ITab
    {
        public Vector2 ScrollPos;

        public Tags(UnitWindow window)
        {

        }

        public void Draw(UnitWindow window)
        {
            ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);
            bool doBreak = false;
            var emptyLabel = new GUIContent("");
            var tagsProperty = window.SerializedObject.FindProperty(nameof(UnitData.Tags));
            for (int i = 0; i < tagsProperty.arraySize; i++)
            {
                var prop = tagsProperty.GetArrayElementAtIndex(i);
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(prop, emptyLabel);

                if (GUILayout.Button("-", GUILayout.Width(30f)))
                {
                    doBreak = true;
                    tagsProperty.DeleteArrayElementAtIndex(i);
                }
                GUILayout.EndHorizontal();

                if (doBreak)
                {
                    break;
                }
            }

            if (GUILayout.Button("+", GUILayout.Height(20f)))
            {
                tagsProperty.arraySize++;
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
