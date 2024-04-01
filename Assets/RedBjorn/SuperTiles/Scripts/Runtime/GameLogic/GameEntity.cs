using RedBjorn.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Game state
    /// </summary>
    [Serializable]
    public class GameEntity
    {
        [SerializeReference]
        public IGameTypeCreator Creator;
        [SerializeReference]
        public IGameTypeLoader Loader;
        public bool Restartable;
        public int RandomSeed;
        public LevelData Level;
        public BattleEntity Battle;

        public static GameEntity Current;

        public event Action OnStarted;

        //Start creation of current session state
        public static void Start()
        {
            CoroutineHelper.Launch(Starting(Current));
        }

        static IEnumerator Starting(GameEntity game)
        {
            if (game != null)
            {
                if (game.Battle != null)
                {
                    yield return Loading(game);
                }
                else
                {
                    yield return Creating(game);
                }
            }
            else
            {
                Log.E("Battle can't start without GameEntity");
            }
        }

        //Creation of а new session
        static IEnumerator Creating(GameEntity game)
        {
            if (game.Creator != null)
            {
                yield return null;
                yield return game.Creator.Create(game);

                ///Make a small delay to have an overview
                yield return new WaitForSeconds(S.Levels.LoadingTimeAfter);

                //Invoke signal that Battle is created
                game.Started();
                Current = null;
            }
            else
            {
                Log.E("Creating aborted. There is no creator for current game type");
            }
        }

        //Load session from an existed battle state (BattleEntity)
        static IEnumerator Loading(GameEntity game)
        {
            if (game.Loader != null)
            {
                yield return null;
                yield return game.Loader.Load(game);

                ///Make a small delay to have an overview
                yield return new WaitForSeconds(S.Levels.LoadingTimeAfter);

                //Invoke signal that Battle is created
                game.Started();
                Current = null;
            }
            else
            {
                Log.E("Loading aborted. There is no loader for current game type");
            }
        }

        void Started()
        {
            OnStarted.SafeInvoke();
        }

        public void Destroy()
        {
            Battle.Destroy();
            Battle = null;
            Log.I("Game destroyed");
        }
    }
}
