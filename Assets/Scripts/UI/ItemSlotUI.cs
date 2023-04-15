using System;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ItemSlotUI: MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private Image _itemImage;
        
        [SerializeField]
        private TMP_Text _itemAmountText;
        
        public InventoryUI UIHandler { get; set; }
        public int SlotIndex { get; set; }
        public Item Item => UIHandler.Inventory.GetItem(SlotIndex);
        
        public static event Action<ItemSlotUI, PointerEventData> OnItemSlotClicked; 

        public void OnPointerDown(PointerEventData eventData)
        {
            OnItemSlotClicked?.Invoke(this, eventData);
        }
        
        public void SetItem(Item item)
        {
            if (item == null)
            {
                _itemImage.sprite = null;
                _itemImage.color = Color.clear;
                _itemAmountText.text = string.Empty;
                return;
            }
            
            _itemImage.sprite = item.ItemData.Icon;
            _itemImage.color = Color.white;
            _itemAmountText.text = item.Amount.ToString();
            
            if (item.Amount == 1)
                _itemAmountText.text = string.Empty;
        }
    }
}