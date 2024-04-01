using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Item.Tabs
{
    public class Tags : ITab
    {
        public Tags(ItemWindow window)
        {

        }

        public void Draw(ItemWindow window)
        {
            EditorGUIUtility.labelWidth = 80f;
            window.ScrollPos = EditorGUILayout.BeginScrollView(window.ScrollPos);
            if (window.Item)
            {
                var statsProperty = window.SerializedObject.FindProperty(nameof(window.Item.Tags));
                for (int j = 0; j < statsProperty.arraySize; j++)
                {
                    GUILayout.BeginHorizontal(GUILayout.Height(20f));
                    var stat = statsProperty.GetArrayElementAtIndex(j);
                    EditorGUILayout.PropertyField(stat, new GUIContent(""), true);
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
                    stat.objectReferenceValue = null;
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
