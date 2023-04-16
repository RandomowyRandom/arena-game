using System;
using Crafting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Crafting
{
    public class CraftingRecipeSlotUI: MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private Image _itemImage;
        
        [SerializeField]
        private Image _slotImage;

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

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            
            OnRecipeSelected?.Invoke(_recipe);
        }
    }
}