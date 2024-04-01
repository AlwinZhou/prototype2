using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Unit.Submenus.Edit.Tabs
{
    public class Actions : ITab
    {
        public Vector2 ScrollPos;

        public Actions(UnitWindow window)
        {

        }

        public void Draw(UnitWindow window)
        {
            ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);
            var serializedObject = window.SerializedObject;
            EditorGUILayout.LabelField("Health Action", EditorStyles.boldLabel);
            RedBjorn.Utils.EditorWindowExtended.DrawProperties(serializedObject.FindProperty(nameof(UnitData.HealthAction)), true);
            EditorGUILayout.Space(10f);
            EditorGUILayout.LabelField("Died Action", EditorStyles.boldLabel);
            RedBjorn.Utils.EditorWindowExtended.DrawProperties(serializedObject.FindProperty(nameof(UnitData.DiedAction)), true);
            EditorGUILayout.Space(10f);
            EditorGUILayout.LabelField("Unit Change Controller Action", EditorStyles.boldLabel);
            RedBjorn.Utils.EditorWindowExtended.DrawProperties(serializedObject.FindProperty(nameof(UnitData.UnitChangeControllerAction)), true);
            EditorGUILayout.EndScrollView();
        }
    }
}
