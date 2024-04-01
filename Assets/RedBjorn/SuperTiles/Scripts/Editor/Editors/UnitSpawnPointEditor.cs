using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors
{
    [CustomEditor(typeof(UnitSpawnPoint))]
    [CanEditMultipleObjects]
    public class UnitSpawnPointInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var spawn = (UnitSpawnPoint)target;
            if (GUILayout.Button("Tile Center"))
            {
                var scene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name;
                var level = LevelData.Find(scene);
                if (level)
                {
                    spawn.transform.position = level.Map.TileCenterWorld(spawn.transform.position);
                }
            }
            if (GUILayout.Button("Unit Editor"))
            {
                UnitWindow.DoShow(spawn.Data);
            }
        }
    }
}
