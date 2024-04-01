using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Game starter point. Wait for scene loading to start level essential creation
    /// </summary>
    public class GameStart : MonoBehaviour
    {
        void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GameEntity.Start();
        }
    }
}
