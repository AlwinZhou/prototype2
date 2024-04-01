using RedBjorn.ProtoTiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Level.Submenus.Tabs
{
    [Serializable]
    public class Map : ITab
    {
        public Vector2 PlayersScroll;
        public List<UnitData> Units;

        public Map(LevelWindow window)
        {

        }

        public void Draw(LevelWindow window)
        {
            PlayersScroll = EditorGUILayout.BeginScrollView(PlayersScroll);
            var common = window.Submenu.GetType() == typeof(Edit);
            if (window.Level && common)
            {
                if (GUILayout.Button("Open Map Editor"))
                {
                    MapWindow.DoShow(window.Level.Map);
                }

                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == window.Level.SceneName)
                {
                    if (Units == null)
                    {
                        Units = ObjectExtensions.FindObjectsOfType<UnitSpawnPoint>().Select(p => p.Data)
                                                                  .OrderBy(p => p.name)
                                                                  .ToList();
                    }

                    EditorGUILayout.Space();
                    foreach (var unit in Units)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.ObjectField(unit, typeof(UnitData), allowSceneObjects: false);
                        if (GUILayout.Button("Edit"))
                        {
                            UnitWindow.DoShow(unit);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                else
                {
                    Units = null;
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField($"Units could be diplayed when\nScene: {window.Level.SceneName}\nwill be loaded", GUILayout.Height(50f));
                    if (!string.IsNullOrEmpty(window.Level.SceneName))
                    {
                        GUI.enabled = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != window.Level.SceneName;
                        if (GUILayout.Button("Load"))
                        {
                            string path = null;
                            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
                            {
                                var scene = EditorBuildSettings.scenes[i];
                                var name = Path.GetFileNameWithoutExtension(scene.path);
                                if (name == window.Level.SceneName)
                                {
                                    path = scene.path;
                                    break;
                                }
                            }

                            if (!string.IsNullOrEmpty(path))
                            {
                                EditorSceneManager.OpenScene(path);
                            }
                        }
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
