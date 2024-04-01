using RedBjorn.Utils;
using System;
using TMPro;
using UnityEngine;

namespace RedBjorn.SuperTiles.UI
{
    public class TurnPlayerMyUI : MonoBehaviour
    {
        public float Delay = 2f;
        public TextMeshProUGUI Text;
        Action OnCompleted;

        void Start()
        {
            Invoke("Despawn", Delay);
        }

        public static void Show(string text, Action onCompleted = null)
        {
            if (S.Battle.UI.TurnStartShowMy)
            {
                Spawner.Spawn(S.Prefabs.TurnStartMyPanel).Init(text, onCompleted);
            }
            else
            {
                onCompleted.SafeInvoke();
            }
        }

        public void Init(string text, Action onCompleted)
        {
            OnCompleted = onCompleted;
            Text.text = text;
        }

        void Despawn()
        {
            Spawner.Despawn(gameObject);
            OnCompleted.SafeInvoke();
        }
    }
}
