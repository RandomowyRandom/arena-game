using System;
using Crafting;
using Inventory.Interfaces;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UI.Crafting.Abstraction;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Crafting
{
    public class CraftingHandlerUI: SerializedMonoBehaviour
    {
        [SerializeField]
        private Image _resultImage;
        
        [SerializeField]
        private Button _craftButton;
        
        [SerializeField]
        private RectTransform _ingredientsPanel;
        
        [SerializeField]
        private CraftingIngredientUI _ingredientPrefab;
        
        [SerializeField]
        private CraftingHandler _craftingHandler;
        
        [OdinSerialize]
        private ICraftingRecipeProvider _recipeProvider;

        private IInventory _playerInventory;
        
        private void Start()
        {
            _playerInventory = ServiceLocator.ServiceLocator.Instance.Get<IPlayerInventory>().Inventory;
            
            _recipeProvider.OnRecipeChanged += RefreshUI;
            
            _craftButton.onClick.AddListener(Craft);
            
            RefreshUI();
        }

        private void Craft()
        {
            var result = _craftingHandler.TryCraft(_recipeProvider.GetRecipe());
            
            RefreshUI();
        }

        private void RefreshUI()
        {
            var recipe = _recipeProvider.GetRecipe();

            _resultImage.sprite = recipe != null ? recipe.Result.ItemData.Icon : null;
            _resultImage.color = recipe == null ? Color.clear : Color.white;
            
            foreach (Transform child in _ingredientsPanel)
            {
                Destroy(child.gameObject);
            }
            
            if(recipe == null)
                return;
            
            foreach (var ingredient in recipe.Ingredients)
            {
                var newIngredient = Instantiate(_ingredientPrefab, _ingredientsPanel);

                var hasEnough = _playerInventory.HasItem(ingredient, out _);
                var isCraftable = _craftingHandler.CanCraft(recipe);
                
                _resultImage.color = isCraftable ? Color.white : new Color(1f, 1f, 1f, 0.33f);

                newIngredient.SetIngredient(ingredient, hasEnough);
            }
        }
    }
}