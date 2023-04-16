using Items;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class CursorItem: MonoBehaviour
    { 
        [SerializeField]
        private Image _itemImage;
        
        [SerializeField]
        private TMP_Text _itemAmountText;

        private Item _heldItem;
        
        private void Awake()
        {
            ItemSlotUI.OnItemSlotClicked += HandleSlotClick;
        }

        private void Start()
        {
            SetItem(null);
        }

        private void OnDestroy()
        {
            ItemSlotUI.OnItemSlotClicked -= HandleSlotClick;
        }

        private void HandleSlotClick(ItemSlotUI slot, PointerEventData eventData)
        {
            if (slot == null)
                return;
            
            var slotHasItem = slot.Item != null && slot.Item.ItemData != null;
            var cursorHasItem = _heldItem != null;
            
            if(!slotHasItem && !cursorHasItem)
                return;

            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    HandleLeftClick(slot, cursorHasItem, slotHasItem);
                    return;
                case PointerEventData.InputButton.Right:
                    HandleRightClick(slot, cursorHasItem, slotHasItem);
                    return;
            }
        }

        private void HandleRightClick(ItemSlotUI slot, bool cursorHasItem, bool slotHasItem)
        {
            if(!cursorHasItem && slotHasItem)
            {
                var amountToTake = Mathf.CeilToInt(slot.Item.Amount / 2f);
                SetItem(new Item(slot.Item.ItemData, amountToTake));
                slot.UIHandler.Inventory.SetItem(slot.SlotIndex, new Item(slot.Item.ItemData, slot.Item.Amount - amountToTake));
                return;
            }
            
            if(cursorHasItem && !slotHasItem)
            {
                slot.UIHandler.Inventory.SetItem(slot.SlotIndex, new Item(_heldItem.ItemData, 1));
                SetItem(new Item(_heldItem.ItemData, _heldItem.Amount - 1));
                
                if(_heldItem.Amount == 0)
                    SetItem(null);
                
                return;
            }
            
            if(cursorHasItem && slotHasItem)
            {
                if(_heldItem.ItemData != slot.Item.ItemData)
                    return;
                
                if(slot.Item.Amount == slot.Item.ItemData.MaxStack)
                    return;
                
                slot.UIHandler.Inventory.SetItem(slot.SlotIndex, new Item(slot.Item.ItemData, slot.Item.Amount + 1));
                SetItem(new Item(_heldItem.ItemData, _heldItem.Amount - 1));
                
                if(_heldItem.Amount == 0)
                    SetItem(null);
            }
        }

        private void HandleLeftClick(ItemSlotUI slot, bool cursorHasItem, bool slotHasItem)
        {
            if (!cursorHasItem && slotHasItem)
            {
                SetItem(slot.Item);
                slot.UIHandler.Inventory.SetItem(slot.SlotIndex, null);
                return;
            }

            if (cursorHasItem && !slotHasItem)
            {
                slot.UIHandler.Inventory.SetItem(slot.SlotIndex, _heldItem);
                SetItem(null);
                return;
            }

            if (cursorHasItem && slotHasItem)
            {
                var cursorItem = _heldItem;
                var slotItem = slot.Item;

                if (cursorItem.ItemData != slotItem.ItemData)
                {
                    SetItem(slotItem);
                    slot.UIHandler.Inventory.SetItem(slot.SlotIndex, cursorItem);
                    return;
                }

                var amountToAdd = Mathf.Min(cursorItem.Amount, slotItem.ItemData.MaxStack - slotItem.Amount);

                slot.UIHandler.Inventory.SetItem(slot.SlotIndex, new Item(slotItem.ItemData, slotItem.Amount + amountToAdd));
                SetItem(new Item(cursorItem.ItemData, cursorItem.Amount - amountToAdd));

                if (_heldItem.Amount == 0)
                    SetItem(null);
            }
        }

        private void SetItem(Item item)
        {
            if (item == null)
            {
                _itemImage.sprite = null;
                _itemImage.color = Color.clear;
                _itemAmountText.text = string.Empty;
                _heldItem = null;
                return;
            }
            
            _itemImage.sprite = item.ItemData.Icon;
            _itemImage.color = Color.white;
            _itemAmountText.text = item.Amount > 1 ? item.Amount.ToString() : string.Empty;
            
            _heldItem = item;
        }
    }
}