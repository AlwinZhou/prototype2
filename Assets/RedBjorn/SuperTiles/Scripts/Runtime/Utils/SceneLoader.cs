using RedBjorn.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Class which implements additive scene loading logic through Loading scene
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        static string SceneName;
        static string SceneName2;
        static Scene Initial;
        public static bool IsLoading;

        static GameObject Prefab { get { return S.Prefabs.LoadingScreen; } }
        static string LoadingName { get { return S.Levels.LoadingSceneName; } }

        public static void Load(string sceneName, string sceneName2 = null)
        {
            SceneName = sceneName;
            SceneName2 = sceneName2;
            SceneManager.LoadScene(LoadingName, LoadSceneMode.Single);
        }

        public static void RemoveLoading()
        {
            if (Initial.IsValid())
            {
                SceneManager.UnloadSceneAsync(Initial);
            }
            else
            {
                Log.E("Can't remove loading. Initial scene is invalid");
            }
        }

        void Awake()
        {
            var loading = Spawner.Spawn(Prefab);
            for (int i = 0; i < loading.transform.childCount; i++)
            {
                loading.transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        void Start()
        {
            if (string.IsNullOrEmpty(SceneName2))
            {
                StartCoroutine(Loading());
            }
            else
            {
                StartCoroutine(Loading2());
            }

        }

        IEnumerator Loading()
        {
            var t = Time.realtimeSinceStartup;
            IsLoading = true;

            var game = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);
            game.allowSceneActivation = false;
            while (game.progress < 0.9f || (Time.realtimeSinceStartup - t) < S.Levels.LoadingTimeMin)
            {
                yield return null;
            }
            game.allowSceneActivation = true;
            IsLoading = false;
        }

        IEnumerator Loading2()
        {
            var t = Time.realtimeSinceStartup;
            IsLoading = true;

            Initial = SceneManager.GetActiveScene();
            var first = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            first.allowSceneActivation = false;
            var second = SceneManager.LoadSceneAsync(SceneName2, LoadSceneMode.Additive);
            second.allowSceneActivation = false;
            while (first.progress < 0.9f && second.progress < 0.9f)
            {
                yield return null;
            }

            var scene = SceneManager.GetSceneByName(SceneName2);
            first.allowSceneActivation = true;
            second.allowSceneActivation = true;

            while (!scene.isLoaded)
            {
                yield return null;
            }
            SceneManager.SetActiveScene(scene);

            while (Time.realtimeSinceStartup - t < S.Levels.LoadingTimeMin)
            {
                yield return null;
            }
            IsLoading = false;
        }

        IEnumerator Unload()
        {
            var currentScene = SceneManager.GetActiveScene();
            var unloadingList = new List<AsyncOperation>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene != currentScene)
                {
                    var unloadingScene = SceneManager.UnloadSceneAsync(scene);
                    unloadingList.Add(unloadingScene);
                }
            }
            var count = unloadingList.Count;
            while (count > 0)
            {
                yield return null;
                for (int i = count - 1; i >= 0; i--)
                {
                    if (unloadingList[i].progress >= 0.9f)
                    {
                        unloadingList.RemoveAt(i);
                    }
                }
                count = unloadingList.Count;
            }
        }
    }
}