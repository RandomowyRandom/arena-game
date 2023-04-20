using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ItemPreview: MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _itemNameText;
        
        [SerializeField]
        private TMP_Text _itemDescriptionText;

        private void Start()
        {
            ItemSlotUI.OnMouseEnterItemSlot += DisplayInfo;
            ItemSlotUI.OnMouseExitItemSlot += ClearInfo;
        }

        private void OnDestroy()
        {
            ItemSlotUI.OnMouseEnterItemSlot -= DisplayInfo;
            ItemSlotUI.OnMouseExitItemSlot -= ClearInfo;
        }

        private void DisplayInfo(ItemSlotUI slot, PointerEventData data)
        {
            if (slot.Item == null)
            {
                ClearInfo(slot, data);
                return;
            }

            _itemNameText.text = slot.Item.ToString();
            _itemDescriptionText.text = slot.Item.ItemData.Description;
            
            _itemNameText.color = slot.Item.IsRarityItem ? slot.Item.GearRarity.Color : Color.white;
        }

        private void ClearInfo(ItemSlotUI slot, PointerEventData data)
        {
            _itemNameText.text = string.Empty;
            _itemDescriptionText.text = string.Empty;
            
            _itemNameText.color = Color.white;
        }
    }
}