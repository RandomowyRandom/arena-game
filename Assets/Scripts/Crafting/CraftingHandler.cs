using System;
using System.Linq;
using Crafting.Abstraction;
using Inventory.Interfaces;
using JetBrains.Annotations;
using Player.Interfaces;
using QFSW.QC;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Crafting
{
    public class CraftingHandler: SerializedMonoBehaviour, ICraftingHandler
    {
        [SerializeField]
        private CraftingRecipeDatabase _craftingRecipeDatabase;

        private IInventory _sourceInventory;
        
        public CraftingRecipeDatabase CraftingRecipeDatabase => _craftingRecipeDatabase;

        private void Start()
        {
            _sourceInventory = ServiceLocator.ServiceLocator.Instance.Get<IPlayerInventory>().Inventory;
        }

        public bool CanCraft(CraftingRecipe recipe)
        {
            var items = _sourceInventory.Items
                .Where(item => item != null);
            
            var ingredients = recipe.Ingredients;
                
            var isCraftable = ingredients
                .All(ingredient => items
                    .Any(i => i.ItemData == ingredient.ItemData && i.Amount >= ingredient.Amount));
                
            return isCraftable;
        }
        
        public bool TryCraft(CraftingRecipe recipe)
        {
            if (!CanCraft(recipe))
                return false;

            var result = recipe.Result;

            if (!_sourceInventory.HasSpaceForItem(result))
                return false;

            var ingredients = recipe.Ingredients;

            foreach (var ingredient in ingredients)
            {
                var leftToRemove = _sourceInventory.TryRemoveItem(ingredient);

                if (leftToRemove != null)
                    return false;
            }
            
            _sourceInventory.TryAddItem(result);
            
            return true;
        }

        #region QC

        [Command("craft")] [UsedImplicitly]
        private void CommandCraft(string recipeKey)
        {
            var recipe = CraftingRecipeDatabase.GetRecipe(recipeKey);
            
            var result = TryCraft(recipe);
            
            Debug.Log($"Crafting result: {result} for item: {recipe.Result.ItemData}");
        }

        #endregion
    }
}