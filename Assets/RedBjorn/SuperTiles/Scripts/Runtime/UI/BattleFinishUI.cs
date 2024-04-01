using RedBjorn.Utils;
using System;
using UnityEngine;

namespace RedBjorn.SuperTiles.UI
{
    public class BattleFinishUI : MonoBehaviour
    {
        public float Delay = 2f;
        Action OnCompleted;

        void Start()
        {
            Invoke("Despawn", Delay);
        }

        public static void Show(Action onCompleted = null)
        {
            if (S.Battle.UI.BattleFinishShow)
            {
                Spawner.Spawn(S.Prefabs.BattleFinishPanel).Init(onCompleted);
            }
            else
            {
                onCompleted.SafeInvoke();
            }
        }

        public void Init(Action onCompleted)
        {
            OnCompleted = onCompleted;
        }

        void Despawn()
        {
            Spawner.Despawn(gameObject);
            OnCompleted.SafeInvoke();
        }
    }
}