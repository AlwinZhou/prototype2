using System;
using UnityEngine;
using UnityEngine.UI;

namespace RedBjorn.SuperTiles.UI
{
    public class MenuGameUI : MonoBehaviour
    {
        public Button CloseButton;
        public Button RestartButton;
        public Button SaveButton;
        public Button LoadButton;
        public Button MenuButton;
        public GameObject Panel;

        BattleView Controller;

        void Awake()
        {
            Hide();
        }

        void Update()
        {
            RestartButton.interactable = Controller.State.IsRestartable();
            SaveButton.interactable = Controller.State.IsSaveable();
            LoadButton.interactable = Controller.State.IsLoadable();
        }

        public void Init(Action onRestartClicked, Action onSaveClicked, Action onLoadClicked, Action onMenuClicked, BattleView controller)
        {
            Controller = controller;

            CloseButton.onClick.RemoveAllListeners();
            CloseButton.onClick.AddListener(Hide);

            RestartButton.onClick.RemoveAllListeners();
            RestartButton.onClick.AddListener(() => onRestartClicked?.Invoke());

            SaveButton.onClick.RemoveAllListeners();
            SaveButton.onClick.AddListener(() => onSaveClicked?.Invoke());

            LoadButton.onClick.RemoveAllListeners();
            LoadButton.onClick.AddListener(() => onLoadClicked?.Invoke());

            MenuButton.onClick.RemoveAllListeners();
            MenuButton.onClick.AddListener(() => onMenuClicked?.Invoke());
        }

        public void Show()
        {
            Panel.SetActive(true);
        }

        public void Hide()
        {
            Panel.SetActive(false);
        }
    }
}