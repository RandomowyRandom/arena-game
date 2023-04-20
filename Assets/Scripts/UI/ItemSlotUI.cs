using System;
using Inventory.Interfaces;
using Items;
using Items.ItemDataSystem;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ItemSlotUI: SerializedMonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Image _itemImage;
        
        [SerializeField]
        private TMP_Text _itemAmountText;
        
        [Space(10)]
        [SerializeField]
        private bool _equipmentLock;
        
        [SerializeField] [ShowIf(nameof(_equipmentLock))]
        private EquipmentType _equipmentType;
        public IInventoryUI UIHandler { get; set; }
        public int SlotIndex { get; set; }
        public bool EquipmentLock => _equipmentLock;
        public EquipmentType EquipmentType => _equipmentType;
        
        public Item Item => UIHandler.Inventory.GetItem(SlotIndex);
        
        public static event Action<ItemSlotUI, PointerEventData> OnItemSlotClicked; 
        public static event Action<ItemSlotUI, PointerEventData> OnMouseEnterItemSlot;
        public static event Action<ItemSlotUI, PointerEventData> OnMouseExitItemSlot; 

        
        public void OnPointerDown(PointerEventData eventData)
        {
            OnItemSlotClicked?.Invoke(this, eventData);
        }
        
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            OnMouseEnterItemSlot?.Invoke(this, eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnMouseExitItemSlot?.Invoke(this, eventData);
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
            
            _itemImage.material = item.IsRarityItem ? item.GearRarity.Material : null;
        }
    }
}