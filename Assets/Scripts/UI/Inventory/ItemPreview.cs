using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class ItemPreview: MonoBehaviour
    {
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
            
            _itemDescriptionText.text = slot.Item.GetTooltip();
        }

        private void ClearInfo(ItemSlotUI slot, PointerEventData data)
        {
            _itemDescriptionText.text = string.Empty;
        }
    }
}