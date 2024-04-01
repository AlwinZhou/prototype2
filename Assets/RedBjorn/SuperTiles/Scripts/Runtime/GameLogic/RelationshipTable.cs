using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedBjorn.SuperTiles
{
    public class RelationshipTable : ScriptableObjectExtended
    {
        [Serializable]
        public class Info 
        {
            public TeamTag Team;
            public List<TeamTag> Allies = new List<TeamTag>();
            public List<TeamTag> Enemies = new List<TeamTag>();
        }

        public List<Info> Table = new List<Info>();

        public const string Suffix = "Relationship";

        public void SetDefault(List<SquadControllerData> controllers)
        {
            Table.Clear();
            foreach (var mainPlayer in controllers)
            {
                var info = new Info();
                info.Team = mainPlayer.Team;
                foreach (var restPlayer in controllers.Where(p => p != mainPlayer))
                {
                    info.Enemies.Add(restPlayer.Team);
                }
                info.Enemies = info.Enemies.OrderBy(e => e.name).ToList();
                Table.Add(info);
            }
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public bool IsAlly(TeamTag main, TeamTag secondary)
        {
            if (main && main == secondary)
            {
                return true;
            }
            var info = Table.FirstOrDefault(t => t.Team == main);
            return info == null ? false : info.Allies.Contains(secondary);
        }

        public bool IsEnemy(TeamTag main, TeamTag secondary)
        {
            if (main && main == secondary)
            {
                return false;
            }
            var info = Table.FirstOrDefault(t => t.Team == main);
            return info == null ? true : info.Enemies.Contains(secondary);
        }

#if UNITY_EDITOR
        public static RelationshipTable Create(string directory, string filename)
        {
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            var relationship = UnityEngine.ScriptableObject.CreateInstance<RelationshipTable>();
            var path = System.IO.Path.Combine(directory, string.Concat(filename, "_", Suffix, FileFormat.Asset));
            path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(path);
            UnityEditor.AssetDatabase.CreateAsset(relationship, path);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            return relationship;
        }
#endif
    }
}
