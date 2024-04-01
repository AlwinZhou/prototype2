using TMPro;
using UnityEngine;

namespace RedBjorn.SuperTiles.UI
{
    public class StatusPanelUI : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        public GameObject Panel;

        BattleView Controller;

        void Awake()
        {
            enabled = Controller;
            Panel.SetActive(enabled);
        }

        void Update()
        {
            Text.text = Controller.Status;
        }

        public void Init(BattleView controller, bool show)
        {
            Controller = controller;
            enabled = show && Controller;
            Panel.SetActive(enabled);
        }
    }
}
