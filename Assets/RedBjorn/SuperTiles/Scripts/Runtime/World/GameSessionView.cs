using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    public class GameSessionView : MonoBehaviour
    {
        GameEntity Game;

        void OnDestroy()
        {
            if (Game != null)
            {
                Game.Destroy();
            }
            Log.I($"{nameof(GameSessionView)} destroyed");
        }

        public void Init(GameEntity game)
        {
            Game = game;
        }
    }
}
