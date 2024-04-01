using System;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Level.Submenus.Tabs
{
    [Serializable]
    public class BattleActions : ITab
    {
        public Vector2 PlayersScroll;

        public BattleActions(LevelWindow window)
        {

        }

        public void Draw(LevelWindow window)
        {
            EditorGUIUtility.labelWidth = window.WindowWidth / 3;
            PlayersScroll = EditorGUILayout.BeginScrollView(PlayersScroll);
            EditorGUILayout.PropertyField(window.SerializedObject.FindProperty(nameof(LevelData.OnBattleStartAction)), true);
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(window.SerializedObject.FindProperty(nameof(LevelData.OnTurnStartAction)), true);
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(window.SerializedObject.FindProperty(nameof(LevelData.OnTurnFinishAction)), true);
            EditorGUILayout.EndScrollView();
        }
    }
}
