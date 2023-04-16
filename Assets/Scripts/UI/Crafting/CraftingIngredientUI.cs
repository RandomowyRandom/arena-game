using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Crafting
{
    public class CraftingIngredientUI: MonoBehaviour
    {
        [SerializeField]
        private Image _itemImage;
        
        [SerializeField]
        private TMP_Text _amountText;
        
        [Header("Item sprite")]
        [SerializeField]
        private Color _hasItemColor = Color.white;
        
        [SerializeField]
        private Color _noItemColor = Color.white;
        
        [Header("Item amount")]
        [SerializeField]
        private Color _hasItemColorAmount = Color.green;
        
        [SerializeField]
        private Color _noItemColorAmount = Color.red;
        
        public void SetIngredient(Item ingredient, bool enough)
        {
            _itemImage.sprite = ingredient.ItemData.Icon;
            _amountText.text = ingredient.Amount.ToString();
            
            _itemImage.color = enough ? _hasItemColor : _noItemColor;
            _amountText.color = enough ? _hasItemColorAmount : _noItemColorAmount;
        }
    }
}