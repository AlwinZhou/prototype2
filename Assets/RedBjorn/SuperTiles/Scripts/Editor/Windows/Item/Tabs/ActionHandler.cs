using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Item.Tabs
{
    public class ActionHandler : ITab
    {
        public ActionHandler(ItemWindow window)
        {


        }

        public void Draw(ItemWindow window)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            var gui = GUI.enabled;
            GUI.enabled = false;
            var labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 90f;
            window.CachedHandler = EditorGUILayout.ObjectField("Action Handler", window.CachedHandler, typeof(RedBjorn.SuperTiles.Items.ActionHandler), allowSceneObjects: false) as RedBjorn.SuperTiles.Items.ActionHandler;
            EditorGUIUtility.labelWidth = labelWidth;
            GUI.enabled = gui;
            GUILayout.EndHorizontal();
            GUILayout.Space(20f);
            window.HandlerSubmenu.Draw(window);

            GUILayout.EndVertical();
        }
    }
}
