using RedBjorn.ProtoTiles;
using RedBjorn.Utils;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RedBjorn.SuperTiles.UI
{
    public class LevelSelectionUI : MonoBehaviour
    {
        public GameObject LevelRef;
        public Transform HexParent;
        public Transform SquareParent;

        void Awake()
        {
            LevelRef.SetActive(false);
            var data = S.Log;
            Log.Init(data.Info, data.Warning, data.Error);
        }

        void Start()
        {
            //Create level buttons for all LevelData which are included in S.Levels
            foreach (var p in S.Levels.Data.OrderBy(p => p.Caption))
            {
                CreateLevelButton(p, ButtonParent(p.Map.Type));
            }
        }

        Transform ButtonParent(GridType grid)
        {
            switch (grid)
            {
                case GridType.HexFlat: return HexParent;
                case GridType.HexPointy: return HexParent;
                case GridType.Square: return SquareParent;
            }
            return null;
        }

        void CreateLevelButton(LevelData level, Transform parent)
        {
            var ui = Spawner.Spawn(LevelRef, parent);
            var text = ui.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);
            var button = ui.GetComponentInChildren<ButtonExtended>(includeInactive: true);
            text.text = level.Caption;
            button.RemoveAllListeners();
            button.AddListener(() => LoadLevel(level));
            ui.SetActive(true);
        }

        void LoadLevel(LevelData level)
        {
            //Create GameEntity state to pass through the scene loading
            GameEntity.Current = new GameEntity
            {
                Creator = new GameTypeCreators.Singleplayer(),
                Loader = new GameTypeLoaders.Singleplayer(),
                Restartable = true,
                Level = level,
                RandomSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue)
            };
            //Load level scene through Loading scene
            SceneLoader.Load(level.SceneName, S.Levels.GameSceneName);
        }
    }
}
