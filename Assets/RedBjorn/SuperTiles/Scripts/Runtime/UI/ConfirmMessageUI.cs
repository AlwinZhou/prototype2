using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RedBjorn.SuperTiles.UI
{
    public class ConfirmMessageUI : MonoBehaviour
    {

        public TextMeshProUGUI MainText;
        public TextMeshProUGUI ConfirmText;
        public TextMeshProUGUI CancelText;
        public GameObject ConfirmHolder;
        public GameObject CancelHolder;
        public ButtonExtended ConfirmButton;
        public ButtonExtended CancelButton;

        Action ConfirmAction;
        Action CancelAction;

        static ConfirmMessageUI Prefab { get { return S.Prefabs.ConfirmMessage; } }
        public static bool IsActive
        {
            get
            {
                Messages.RemoveAll(m => m == null);
                return Messages.Count > 0;
            }
        }

        static List<ConfirmMessageUI> Messages = new List<ConfirmMessageUI>();

        public static void Show(string text, string confirmText, string cancelText, Action confirmAction, Action cancelAction)
        {
            var message = Spawner.Spawn(Prefab);
            message.MainText.text = text;
            message.ConfirmText.text = confirmText;
            message.ConfirmAction = confirmAction;
            message.CancelAction = cancelAction;
            message.ConfirmHolder.gameObject.SetActive(true);
            if (!string.IsNullOrEmpty(cancelText))
            {
                message.CancelText.text = cancelText;
                message.CancelHolder.SetActive(true);
            }
            else
            {
                message.CancelHolder.SetActive(false);
            }
            Messages.Add(message);
        }

        void OnConfirm()
        {
            ConfirmAction.SafeInvoke();
            Messages.Remove(this);
            Spawner.Despawn(gameObject);
        }

        void OnCancel()
        {
            CancelAction.SafeInvoke();
            Messages.Remove(this);
            Spawner.Despawn(gameObject);
        }

        void Awake()
        {
            ConfirmButton.RemoveAllListeners();
            ConfirmButton.AddListener(OnConfirm);
            CancelButton.RemoveAllListeners();
            CancelButton.AddListener(OnCancel);
        }

        void Update()
        {
            var ignoreConfirmMessage = Messages[Messages.Count - 1] == this;
            if (InputController.GetGameHotkeyUp(S.Input.Submit, ignoreConfirmMessage))
            {
                OnConfirm();
            }
            else if (InputController.GetGameHotkeyUp(S.Input.Cancel, ignoreConfirmMessage))
            {
                OnCancel();
            }
        }
    }
}
