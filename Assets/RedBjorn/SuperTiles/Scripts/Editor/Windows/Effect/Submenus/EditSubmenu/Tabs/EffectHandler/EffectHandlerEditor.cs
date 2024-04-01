using RedBjorn.SuperTiles.Effects;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Effect.Tabs
{
    public interface IEffectHandlerEditor
    {
        void Draw(EffectHandlerEditor tab, EffectWindow window);
    }

    public class EffectHandlerEditor
    {
        public IEffectHandlerEditor Menu;
        public EffectHandlerInfo Info;
        public string Suffix;

        public EffectHandlerEditor(EffectHandlerInfo info, string suffix)
        {
            Info = info;
            Suffix = suffix;
            Menu = new Edit();
        }

        public void Draw(EffectWindow window)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            var gui = GUI.enabled;
            GUI.enabled = false;
            var labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 90f;
            if (Info != null)
            {
                Info.Effect = EditorGUILayout.ObjectField("Effect Handler", Info.Effect, typeof(EffectHandler), allowSceneObjects: false) as EffectHandler;
            }
            EditorGUIUtility.labelWidth = labelWidth;
            GUI.enabled = gui;
            GUILayout.EndHorizontal();
            GUILayout.Space(20f);
            Menu.Draw(this, window);
            GUILayout.EndVertical();
        }
    }
}
