using RedBjorn.Utils;
using System;
using TMPro;
using UnityEngine;

namespace RedBjorn.SuperTiles.UI
{
    public class TurnPanelUI : MonoBehaviour
    {
        public TextMeshProUGUI Turn;
        public GameObject Panel;
        public ButtonExtended CompleteTurn;

        BattleView Controller;

        void Awake()
        {
            enabled = Controller;
            Panel.SetActive(enabled);
        }

        void Update()
        {
            if (Controller.Battle != null)
            {
                Turn.text = $"Turn: {Controller.Battle.Turn.ToString()}";
            }
        }

        public void Init(BattleView controller, Action onCompleteTurn, bool showTurn)
        {
            Controller = controller;
            enabled = Controller && showTurn;
            Panel.SetActive(enabled);

            CompleteTurn.RemoveAllListeners();
            CompleteTurn.AddListener(() => onCompleteTurn.SafeInvoke());
        }
    }
}
