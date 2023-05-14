using System;
using Crafting;
using Crafting.Abstraction;
using Inventory.Interfaces;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UI.Crafting.Abstraction;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Crafting
{
    public class CraftingHandlerPreview: SerializedMonoBehaviour
    {
        [SerializeField]
        private Transform _ingredientsPanel;
        
        [SerializeField]
        private CraftingIngredientPreview _ingredientPrefab;
        
        [OdinSerialize]
        private ICraftingHandler _craftingHandler;
        
        [OdinSerialize]
        private ICraftingRecipeProvider _recipeProvider;

        private IInventory _playerInventory;
        
        private void Start()
        {
            _playerInventory = ServiceLocator.ServiceLocator.Instance.Get<IPlayerInventory>().Inventory;
            
            _recipeProvider.OnRecipeChanged += RefreshUI;
            
            RefreshUI();
        }

        public void Craft()
        {
            var result = _craftingHandler.TryCraft(_recipeProvider.GetRecipe());
            
            RefreshUI();
        }

        private void RefreshUI()
        {
            const int maxHorizontalSize = 3;
            const int maxVerticalSize = 2;
            
            const float xIncrement = .6f;
            const float yIncrement = -.45f;
            
            var x = 0f;
            var y = 0f;
            
            var xItemAmount = 0;
            var yItemAmount = 0;
            
            var recipe = _recipeProvider.GetRecipe();

            foreach (Transform child in _ingredientsPanel)
            {
                Destroy(child.gameObject);
            }
            
            if(recipe == null)
                return;
            
            foreach (var ingredient in recipe.Ingredients)
            {
                var newIngredient = Instantiate(_ingredientPrefab, _ingredientsPanel);

                if(xItemAmount >= maxHorizontalSize)
                {
                    xItemAmount = 0;
                    x = 0;
                    yItemAmount++;
                    y += yIncrement;
                }
                
                newIngredient.transform.localPosition = new Vector3(x, y, 0);
                
                var hasEnough = _playerInventory.HasItem(ingredient, out _);
                
                newIngredient.SetIngredient(ingredient, hasEnough);
                
                xItemAmount++;
                x += xIncrement;
            }
        }
    }
}