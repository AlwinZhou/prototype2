#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RedBjorn.Utils
{
    public class Tag : ScriptableObjectExtended
    {
#if UNITY_EDITOR
        public static T Find<T>(string filter) where T : Tag
        {

            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name} {filter}");
            if (guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                var data = AssetDatabase.LoadAssetAtPath<T>(path);
                return data;
            }

            guids = AssetDatabase.FindAssets(string.Concat("t:", typeof(T).Name));
            if (guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                var data = AssetDatabase.LoadAssetAtPath<T>(path);
                return data;
            }

            return null;
        }
#endif
    }
}
