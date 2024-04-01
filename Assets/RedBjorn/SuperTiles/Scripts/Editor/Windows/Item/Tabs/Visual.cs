using RedBjorn.Utils;
using UnityEditor;

namespace RedBjorn.SuperTiles.Editors.Item.Tabs
{
    public class Visual : ITab
    {
        public Visual(ItemWindow window)
        {

        }

        public void Draw(ItemWindow window)
        {
            EditorGUIUtility.labelWidth = 135f;
            if (window.SerializedObject != null)
            {
                window.ScrollPos = EditorGUILayout.BeginScrollView(window.ScrollPos);
                EditorGUILayout.PropertyField(window.SerializedObject.FindProperty(nameof(ItemData.Caption)));
                EditorGUILayout.PropertyField(window.SerializedObject.FindProperty(nameof(ItemData.Description)));
                EditorGUILayout.Space(10);
                EditorWindowExtended.DrawProperties(window.SerializedObject.FindProperty(nameof(ItemData.Visual)), true);
                EditorGUILayout.EndScrollView();
            }
        }
    }
}
