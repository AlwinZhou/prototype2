using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors
{
    [CustomEditor(typeof(ItemData))]
    [CanEditMultipleObjects]
    public class ItemDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Item Editor"))
            {
                ItemWindow.DoShow((ItemData)target);
            }
            base.OnInspectorGUI();
        }
    }
}