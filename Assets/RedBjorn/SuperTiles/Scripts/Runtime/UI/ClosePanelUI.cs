using RedBjorn.Utils;
using System;
using UnityEngine;

namespace RedBjorn.SuperTiles.UI
{
    public class ClosePanelUI : MonoBehaviour
    {
        [SerializeField]
        ButtonExtended CloseButton;

        public void Init(Action onClose)
        {
            CloseButton.RemoveAllListeners();
            CloseButton.AddListener(() => onClose.SafeInvoke());
        }
    }
}