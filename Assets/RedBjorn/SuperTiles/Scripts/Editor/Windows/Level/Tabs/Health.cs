using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Level.Submenus.Tabs
{
    [Serializable]
    public class Health : ITab
    {
        public Vector2 PlayersScroll;

        public Health(LevelWindow window)
        {

        }

        public void Draw(LevelWindow window)
        {
            EditorGUIUtility.labelWidth = 50f;
            GUI.enabled = false;
            window.CachedHealth = EditorGUILayout.ObjectField("Health", window.CachedHealth, typeof(SuperTiles.HealthConvertRules), allowSceneObjects: false) as SuperTiles.HealthConvertRules;
            GUI.enabled = true;
            EditorGUILayout.Space();
            if (window.SerializedHealth != null && window.SerializedHealth.targetObject != null)
            {
                PlayersScroll = EditorGUILayout.BeginScrollView(PlayersScroll);
                bool doBreak = false;
                var playersProperty = window.SerializedHealth.FindProperty(nameof(HealthConvertRules.Rules));
                for (int i = 0; i < playersProperty.arraySize; i++)
                {
                    var prop = playersProperty.GetArrayElementAtIndex(i);

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(string.Format("Rule {0}", i + 1), GUILayout.Width(44f));
                    EditorGUILayout.PropertyField(prop, new GUIContent(""), true);
                    if (GUILayout.Button("Edit", GUILayout.Height(20f), GUILayout.Width(40f)))
                    {
                        HealthRuleWindow.DoShow(prop.objectReferenceValue as ConvertRule);
                    }
                    if (GUILayout.Button("-", GUILayout.Height(20f), GUILayout.Width(40f)))
                    {
                        doBreak = true;
                        if (prop.objectReferenceValue != null)
                        {
                            playersProperty.DeleteArrayElementAtIndex(i);
                        }
                        playersProperty.DeleteArrayElementAtIndex(i);
                    }
                    GUILayout.EndHorizontal();

                    EditorGUILayout.Space();
                    if (doBreak)
                    {
                        break;
                    }
                }

                if (GUILayout.Button("+", GUILayout.Height(20f)))
                {
                    playersProperty.arraySize++;
                }

                window.SerializedHealth.ApplyModifiedProperties();
                EditorGUILayout.EndScrollView();
            }
            else
            {
                if (GUILayout.Button("Create", GUILayout.Height(20f)))
                {
                    var assetPath = AssetDatabase.GetAssetPath(window.Level);
                    var folderPath = Path.GetDirectoryName(assetPath);
                    var filename = Path.GetFileNameWithoutExtension(assetPath);
                    window.CachedRootFolderPath = folderPath;
                    window.CachedLevelName = filename;
                    var health = HealthConvertRules.Create(window.CachedRootFolderPath, window.CachedLevelName);
                    window.CachedHealth = health;
                    window.Level.Health = window.CachedHealth;
                    window.SerializedHealth = window.CachedHealth ? new SerializedObject(window.CachedHealth) : null;
                    EditorUtility.SetDirty(window.Level);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        }
    }
}
