using RedBjorn.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RedBjorn.SuperTiles.UI
{
    public class ButtonExtended : Button
    {
        public bool CommonSound = true;
        [SerializeField]
        AudioClip Clip;

        public void RemoveAllListeners()
        {
            onClick.RemoveAllListeners();
            if (CommonSound)
            {
                onClick.AddListener(() => AudioController.PlayButtonClick());
            }
            else
            {
                onClick.AddListener(() => AudioController.PlaySound(Clip));
            }
        }

        public void AddListener(Action action)
        {
            onClick.AddListener(() => action.SafeInvoke());
        }
    }
}