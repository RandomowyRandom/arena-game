using System;
using Crafting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Crafting
{
    public class CraftingRecipePreview: MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _itemImage;
        
        [SerializeField]
        private SpriteRenderer _slotImage;

        [SerializeField]
        private Color _craftableColor = Color.white;
        
        [SerializeField]
        private Color _availableColor = Color.white;
        
        public event Action<CraftingRecipe> OnRecipeSelected;

        private CraftingRecipe _recipe;
        
        public CraftingRecipe Recipe => _recipe;
        
        public void SetRecipe(CraftingRecipe recipe, bool isCraftable)
        {
            _recipe = recipe;
            _itemImage.sprite = recipe.Result.ItemData.Icon;
            
            _itemImage.color = isCraftable ? _craftableColor : _availableColor;
            _slotImage.color = isCraftable ? _craftableColor : _availableColor;
        }
    }
}