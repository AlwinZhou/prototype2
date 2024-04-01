using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Unit.Submenus.Edit.Tabs
{
    public class Visual : ITab
    {
        public Vector2 ScrollPos;

        public Visual(UnitWindow window)
        {

        }

        public void Draw(UnitWindow window)
        {
            ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);
            var serializedObject = window.SerializedObject;
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(UnitData.Avatar)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(UnitData.Model)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(UnitData.Collider)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(UnitData.UiHolder)));
            EditorGUILayout.EndScrollView();
        }
    }
}
