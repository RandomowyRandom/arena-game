using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Crafting
{
    public class CraftingIngredientPreview: MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _itemImage;
        
        [SerializeField]
        private TMP_Text _amountText;
        
        [Header("Item amount")]
        [SerializeField]
        private Color _hasItemColorAmount = Color.green;
        
        [SerializeField]
        private Color _noItemColorAmount = Color.red;
        
        public void SetIngredient(Item ingredient, bool enough)
        {
            _itemImage.sprite = ingredient.ItemData.Icon;
            _amountText.text = ingredient.Amount.ToString();
            
            _amountText.color = enough ? _hasItemColorAmount : _noItemColorAmount;
        }
    }
}