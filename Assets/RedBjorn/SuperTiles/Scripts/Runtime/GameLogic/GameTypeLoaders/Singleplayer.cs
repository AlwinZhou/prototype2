using RedBjorn.ProtoTiles;
using RedBjorn.SuperTiles.Squad;
using RedBjorn.SuperTiles.Utils;
using RedBjorn.Utils;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.GameTypeLoaders
{
    [Serializable]
    public class Singleplayer : IGameTypeLoader
    {
        public IEnumerator Load(GameEntity game)
        {
            var battleView = ObjectExtensions.FindObjectOfType<BattleView>();
            if (!battleView)
            {
                Log.E($"Can't create battle without {nameof(BattleView)} on scene");
                yield break;
            }

            game.Battle.RandomCreate(game.RandomSeed);
            game.Battle.Game = game;
            game.Battle.Level = game.Level;
            game.Battle.TurnPlayer = new TurnPlayer(game.Battle);
            var sessionGo = new GameObject("GameSession");
            var session = sessionGo.AddComponent<GameSessionView>();
            session.Init(game);
            var level = game.Level;
            var map = level.Map;
            var mapVisual = ObjectExtensions.FindObjectOfType<MapView>();
            //Create state for MapSettings
            game.Battle.Map = new MapEntity(map, mapVisual);

            //Add alive units to the map
            foreach (var u in game.Battle.UnitsAlive)
            {
                game.Battle.Map.RegisterUnit(u);
            }

            if (mapVisual != null)
            {
                mapVisual.Init(game.Battle.Map);
            }
            else
            {
                Log.W($"Can't find {nameof(MapView)}. Some map functions will be disabled");
            }

            //Load all players
            foreach (var player in game.Battle.Players)
            {
                player.Load(game);
            }

            //Load all alive units
            foreach (var unit in game.Battle.UnitsAlive)
            {
                unit.Load(game);
                unit.Data.GetUnitChangeControllerAction()?.Handle(unit, game.Battle.Players.FirstOrDefault(p => p.Squad.Contains(unit)), game.Battle);
            }

            //Select current unit
            if (game.Battle.Unit == null)
            {
                game.Battle.Unit = game.Battle.UnitsTimeline[0];
            }

            //Select current player
            if (game.Battle.Player == null)
            {
                game.Battle.Player = game.Battle.Players.FirstOrDefault(p => p.Squad.Contains(game.Battle.Unit));
            }

            //Send root state object to view of BattleEntity
            battleView.Init(game, game.Battle.Players.Where(p => p is PlayerEntity).ToList());

            //Create predefined main camera or change values of existed camera
            Camera camera = null;
            if (level.Camera.Prefab)
            {
                if (Camera.main)
                {
                    Spawner.Despawn(Camera.main.gameObject);
                }
                var cameraGo = Spawner.Spawn(level.Camera.Prefab);
                camera = cameraGo.GetComponent<Camera>();
                camera.tag = "MainCamera";
            }
            else
            {
                camera = Camera.main;
            }
            camera.transform.position = level.Camera.StartPosition;
            camera.transform.rotation = Quaternion.Euler(level.Camera.StartRotation);
            if (camera.TryGetComponent(out CameraController cameraController))
            {
                cameraController.Init(game.Battle.Map);
            }
            if (camera.TryGetComponent(out RotatorAround rotator))
            {
                rotator.Init(map.Plane());
            }
            var detector = ObjectExtensions.FindObjectOfType<InteractableDetector>();
            if (detector)
            {
                detector.Init(map.Plane());
            }

            //Wait until loading screen will disable
            while (SceneLoader.IsLoading)
            {
                yield return null;
            }
            SceneLoader.RemoveLoading();
        }
    }
}