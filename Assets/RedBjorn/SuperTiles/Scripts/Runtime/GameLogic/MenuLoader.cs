using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Load Menu scene when there is not LevelEntity current state
    /// </summary>
    public class MenuLoader : MonoBehaviour
    {
#if UNITY_EDITOR
        Settings.LevelSettings Levels => S.Levels;

        void Start()
        {
            if (GameEntity.Current == null)
            {
                if (Levels.LoadMenuWhenLevelStarts)
                {
                    SceneManager.LoadScene(Levels.MenuSceneName);
                }
                else
                {
                    var scene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
                    var level = LevelData.Find(scene.name);
                    if (!level)
                    {
                        return;
                    }
                    GameEntity.Current = new GameEntity
                    {
                        Creator = new GameTypeCreators.Singleplayer(),
                        Loader = new GameTypeLoaders.Singleplayer(),
                        Restartable = true,
                        Level = level,
                        RandomSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue)
                    };
                    SceneLoader.Load(level.SceneName, Levels.GameSceneName);
                }
            }
        }

        public static MenuLoader Create()
        {
            var go = UnityEditor.PrefabUtility.InstantiatePrefab(S.Prefabs.MenuLoader) as MenuLoader;
            go.gameObject.name = "MenuLoader";
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            return go.GetComponent<MenuLoader>();
        }
#endif
    }
}
