using RedBjorn.Utils;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RedBjorn.SuperTiles.UI
{
    /// <summary>
    /// Class which represents Item inside UI
    /// </summary>
    public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        [SerializeField]
        Color SelectedColor;
        [SerializeField]
        Color InteractableColor;
        [SerializeField]
        Color UninteractableColor;
        [SerializeField]
        Image ItemIcon;
        [SerializeField]
        Image BackgroundIcon;
        [SerializeField]
        ButtonExtended ActivateButton;
        [SerializeField]
        GameObject StackPanel;
        [SerializeField]
        GameObject SelectBorder;
        [SerializeField]
        TextMeshProUGUI StackCount;

        public ItemEntity Item { get; private set; }
        bool Active;
        SessionTooltipUI Tooltip;

        void OnDisable()
        {
            TooltipUnregister();
        }

        public void Init(ItemEntity item, bool active, Action<ItemEntity> onStartActivation)
        {
            Item = item;
            Active = active;
            LoadItem();

            ActivateButton.RemoveAllListeners();
            if (onStartActivation != null)
            {
                ActivateButton.AddListener(() =>
                {
                    onStartActivation.SafeInvoke(item);
                    ActivateButton.interactable = false;
                });
            }
            Tooltip = ObjectExtensions.FindObjectOfType<SessionTooltipUI>();
            UpdateState();
        }

        public void Deactivate(ItemEntity item)
        {
            Item = item;

            ActivateButton.RemoveAllListeners();
            UninteractableState();

            LoadItem();
        }

        public void Deactivate()
        {
            ActivateButton.RemoveAllListeners();
            UninteractableState();
        }

        public void LoadItem()
        {
            if (Item != null)
            {
                ItemIcon.sprite = Item.Data.Visual.IconSmall;
                if (Item.Data.Visual.UseColor)
                {
                    BackgroundIcon.color = Item.Data.Visual.Color;
                }
                gameObject.name = Item.Data.name;
                if (Item.Data.Stackable)
                {
                    StackCount.text = string.Format("x{0}", Item.CurrentStackCount);
                    StackPanel.SetActive(true);
                }
                else
                {
                    StackCount.text = string.Empty;
                    StackPanel.SetActive(false);
                }

            }
        }

        public void SelectState()
        {
            SelectBorder.SetActive(true);
            ItemIcon.color = SelectedColor;
            ActivateButton.interactable = false;
        }

        public void UninteractableState()
        {
            SelectBorder.SetActive(false);
            ItemIcon.color = UninteractableColor;
            ActivateButton.interactable = false;
        }

        public void InteractableState()
        {
            SelectBorder.SetActive(false);
            ItemIcon.color = InteractableColor;
            ActivateButton.interactable = true;
        }

        public void UpdateState()
        {
            if (Active)
            {
                InteractableState();
            }
            else
            {
                UninteractableState();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Tooltip)
            {
                Tooltip.Register(Item);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipUnregister();
        }

        void TooltipUnregister()
        {
            if (Tooltip)
            {
                Tooltip.Unregister(Item);
            }
        }
    }
}