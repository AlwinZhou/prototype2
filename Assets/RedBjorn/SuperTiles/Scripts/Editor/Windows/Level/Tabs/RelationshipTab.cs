using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors.Level.Submenus.Tabs
{
    [Serializable]
    public class RelationshipTab : ITab
    {
        public Vector2 PlayersScroll;

        public RelationshipTab(LevelWindow window)
        {

        }

        public void Draw(LevelWindow window)
        {
            GUI.enabled = false;
            window.CachedRelationship = EditorGUILayout.ObjectField("Relationship", window.CachedRelationship, typeof(RelationshipTable), allowSceneObjects: false) as RelationshipTable;
            GUI.enabled = true;
            if (!window.CachedRelationship)
            {
                if (GUILayout.Button("Create"))
                {
                    if (window.Level)
                    {
                        var levelPath = AssetDatabase.GetAssetPath(window.Level);
                        var directory = Path.GetDirectoryName(levelPath);
                        var assetName = string.IsNullOrEmpty(window.Level.SceneName) ? "Level" : window.Level.SceneName;
                        var relationship = RelationshipTable.Create(directory, assetName);
                        if (relationship)
                        {
                            relationship.SetDefault(window.Level.Players);
                            window.CachedRelationship = relationship;
                            window.SerializedRelationship = window.CachedRelationship ? new SerializedObject(window.CachedRelationship) : null;
                        }
                        window.Level.Relationship = relationship;
                        EditorUtility.SetDirty(window.Level);
                        AssetDatabase.SaveAssets();
                    }
                }
            }
            GUILayout.Space(20f);
            PlayersScroll = EditorGUILayout.BeginScrollView(PlayersScroll);
            var serialized = window.SerializedRelationship;
            if (serialized != null && serialized.targetObject)
            {
                EditorGUIUtility.labelWidth = 110f;
                var prop = serialized.GetIterator();
                prop.NextVisible(true);
                while (prop.NextVisible(true))
                {
                    if (prop.depth == 0)
                    {
                        EditorGUILayout.PropertyField(prop, true);
                    }
                }
                serialized.ApplyModifiedProperties();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
