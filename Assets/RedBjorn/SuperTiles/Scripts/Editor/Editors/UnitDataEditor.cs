using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors
{
    [CustomEditor(typeof(UnitData))]
    [CanEditMultipleObjects]
    public class UnitDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Unit Editor"))
            {
                UnitWindow.DoShow((UnitData)target);
            }
            base.OnInspectorGUI();
        }
    }
}
