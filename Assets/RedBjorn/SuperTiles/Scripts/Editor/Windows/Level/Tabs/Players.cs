using System;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Level.Submenus.Tabs
{
    [Serializable]
    public class Players : ITab
    {
        public Vector2 PlayersScroll;
        public Players(LevelWindow window)
        {

        }

        public void Draw(LevelWindow window)
        {
            GUI.enabled = true;
            if (window.Level)
            {
                PlayersScroll = EditorGUILayout.BeginScrollView(PlayersScroll);
                EditorGUIUtility.labelWidth = 70f;
                bool doBreak = false;
                var playersProperty = window.SerializedObject.FindProperty(nameof(LevelData.Players));
                for (int i = 0; i < playersProperty.arraySize; i++)
                {
                    var prop = playersProperty.GetArrayElementAtIndex(i);
                    EditorGUILayout.LabelField(string.Format("Player {0}", i + 1));

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    EditorGUILayout.PropertyField(prop.FindPropertyRelative(nameof(SquadControllerData.Name)), new GUIContent("Name"), true);
                    EditorGUILayout.PropertyField(prop.FindPropertyRelative(nameof(SquadControllerData.Team)), new GUIContent("Team"), true);
                    EditorGUILayout.PropertyField(prop.FindPropertyRelative(nameof(SquadControllerData.ControlledBy)), new GUIContent("Manager"), true);
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(GUILayout.Width(30f));
                    if (GUILayout.Button("-", GUILayout.Height(60f)))
                    {
                        doBreak = true;
                        playersProperty.DeleteArrayElementAtIndex(i);
                    }
                    GUILayout.EndVertical();
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

                EditorGUILayout.EndScrollView();
            }
        }
    }
}
