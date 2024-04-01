using System;
using TMPro;
using UnityEngine;

namespace RedBjorn.SuperTiles.UI
{
    public class TurnDurationUI : MonoBehaviour
    {
        public TextMeshProUGUI Duration;
        public GameObject Panel;

        BattleView Controller;

        void Awake()
        {
            enabled = Controller;
            Panel.SetActive(enabled);
        }

        void Update()
        {
            if (Controller.Battle != null && Controller.Battle.TurnDuration > 0)
            {
                var duration = Controller.Battle.TurnDuration - Controller.Battle.TurnPlayer.TimePassed;
                duration = Mathf.Max(duration, 0f);
                var durationText = $"[{TimeSpan.FromSeconds(duration).ToString(@"mm\:ss")}]";
                Duration.text = durationText;
                Panel.SetActive(Controller.Battle.State != BattleState.Finished);
            }
            else
            {
                Panel.SetActive(false);
            }
        }

        public void Init(BattleView controller, bool showTurn)
        {
            Controller = controller;
            enabled = Controller && showTurn;
            Panel.SetActive(enabled);
        }
    }
}
