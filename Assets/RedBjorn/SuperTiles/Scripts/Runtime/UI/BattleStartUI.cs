using RedBjorn.Utils;
using System;
using UnityEngine;

namespace RedBjorn.SuperTiles.UI
{
    public class BattleStartUI : MonoBehaviour
    {
        public ButtonExtended BattleStart;

        public void Init(Action onBattleStart)
        {
            BattleStart.RemoveAllListeners();
            if (onBattleStart == null)
            {
                BattleStart.gameObject.SetActive(false);
            }
            else
            {
                BattleStart.AddListener(() =>
                {
                    BattleStart.gameObject.SetActive(false);
                    onBattleStart.SafeInvoke();
                });
            }
        }
    }
}