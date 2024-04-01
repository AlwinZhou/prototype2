using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Item.Tabs
{
    public class AvailableCondition : ITab
    {
        public AvailableCondition(ItemWindow window)
        {

        }

        public void Draw(ItemWindow window)
        {
            EditorGUIUtility.labelWidth = window.WindowWidth / 3;
            window.ScrollPos = EditorGUILayout.BeginScrollView(window.ScrollPos);
            RedBjorn.Utils.EditorWindowExtended.DrawProperties(window.SerializedObject.FindProperty(nameof(ItemData.AvailableCondition)), true);
            EditorGUILayout.EndScrollView();
        }
    }
}
