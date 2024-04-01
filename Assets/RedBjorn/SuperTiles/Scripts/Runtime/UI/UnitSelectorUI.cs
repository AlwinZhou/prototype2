using RedBjorn.Utils;
using System;
using TMPro;
using UnityEngine;

namespace RedBjorn.SuperTiles.UI
{
    public class UnitSelectorUI : MonoBehaviour
    {
        public ButtonExtended SelectButton;
        public TextMeshProUGUI Text;
        public GameObject Selected;

        UnitEntity Unit;


        public void Init(Action<UnitEntity> onSelect, UnitEntity unit, int index, bool selected)
        {
            Unit = unit;

            Text.text = index.ToString();
            SelectButton.RemoveAllListeners();
            SelectButton.AddListener(() => onSelect.SafeInvoke(Unit));
            if (selected)
            {
                Selected.SetActive(true);
            }
            else
            {
                Selected.SetActive(false);
            }

            gameObject.SetActive(true);
        }
    }
}