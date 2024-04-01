using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Unit.Submenus.Edit.Tabs
{
    public class DefaultItems : ITab
    {
        public Vector2 ScrollPos;

        public DefaultItems(UnitWindow window)
        {

        }

        public void Draw(UnitWindow window)
        {
            ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);
            var doBreak = false;
            var SerializedObject = window.SerializedObject;
            var statsProperty = SerializedObject.FindProperty(nameof(UnitData.DefaultItems));
            EditorGUIUtility.labelWidth = 20f;
            for (int i = 0; i < statsProperty.arraySize; i++)
            {
                var prop = statsProperty.GetArrayElementAtIndex(i);
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(prop, new GUIContent(i.ToString()));

                if (GUILayout.Button("Edit", GUILayout.Width(35f)))
                {
                    var folder = window.CachedRootFolderPath;
                    var item = prop.objectReferenceValue as ItemData;
                    if (string.IsNullOrEmpty(folder))
                    {
                        if (item)
                        {
                            folder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(item));
                        }
                        else
                        {
                            folder = string.Empty;
                        }
                    }
                    else
                    {
                        folder = Path.Combine(window.CachedRootFolderPath, @"..", "Items");
                    }
                    ItemWindow.DoShow(prop.objectReferenceValue as ItemData, folder);

                }
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
