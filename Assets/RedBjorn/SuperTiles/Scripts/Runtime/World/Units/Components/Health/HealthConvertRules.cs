using RedBjorn.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Convert Rules storage
    /// </summary>
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Health.RulesAsset)]
    public class HealthConvertRules : ScriptableObjectExtended
    {
        public const string Suffix = "HealthRules";

        public List<ConvertRule> Rules = new List<ConvertRule>();

        public float Handle(float delta, UnitEntity victim, UnitEntity damager, ItemEntity item)
        {
            var current = delta;
            foreach (var rule in Rules.Where(r => r))
            {
                current = rule.Convert(current, victim, damager, item);
            }
            return current;
        }

#if UNITY_EDITOR
        public static HealthConvertRules Create(string directory, string filename)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var health = ScriptableObject.CreateInstance<HealthConvertRules>();
            var path = Path.Combine(directory, string.Concat(filename, "_", HealthConvertRules.Suffix, FileFormat.Asset));
            path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(path);
            UnityEditor.AssetDatabase.CreateAsset(health, path);
            return health;
        }
#endif
    }
}
