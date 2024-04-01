using System;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Level.Submenus.Tabs
{
    [Serializable]
    public class OtherTab : ITab
    {
        public Vector2 PlayersScroll;

        public OtherTab(LevelWindow window)
        {

        }

        public void Draw(LevelWindow window)
        {
            PlayersScroll = EditorGUILayout.BeginScrollView(PlayersScroll);
            GUI.enabled = true;
            EditorGUIUtility.labelWidth = window.WindowWidth / 3;
            if (window.SerializedObject != null && window.SerializedObject.targetObject != null)
            {
                EditorGUILayout.PropertyField(window.SerializedObject.FindProperty(nameof(LevelData.AutoStart)), true);
                EditorGUILayout.PropertyField(window.SerializedObject.FindProperty(nameof(LevelData.EqualEffectInfluenceDuration)), true);
                EditorGUILayout.PropertyField(window.SerializedObject.FindProperty(nameof(LevelData.TeamMarkersEnable)), true);
                EditorGUILayout.PropertyField(window.SerializedObject.FindProperty(nameof(LevelData.MarkerAlly)), true);
                EditorGUILayout.PropertyField(window.SerializedObject.FindProperty(nameof(LevelData.MarkerEnemy)), true);
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
