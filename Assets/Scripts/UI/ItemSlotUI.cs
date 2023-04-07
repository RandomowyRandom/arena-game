using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemSlotUI: MonoBehaviour
    {
        [SerializeField]
        private Image _itemImage;
        
        [SerializeField]
        private TMP_Text _itemAmountText;
        
        public void SetItem(Item item)
        {
            if (item == null)
            {
                _itemImage.sprite = null;
                _itemAmountText.text = string.Empty;
                return;
            }
            
            _itemImage.sprite = item.ItemData.Icon;
            _itemAmountText.text = item.Amount.ToString();
        }
    }
}