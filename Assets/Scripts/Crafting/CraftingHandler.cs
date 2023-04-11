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

        [Space(50)]
        [Header("====================TESTING====================")]
        [InfoBox("This is just for testing purposes")]
        [OdinSerialize]
        private CraftingRecipe _testRecipe;
        
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

            if (!_sourceInventory.HasSpaceForItem(recipe.Result))
                return false;

            var ingredients = recipe.Ingredients;

            foreach (var ingredient in ingredients)
            {
                var leftToRemove = _sourceInventory.TryRemoveItem(ingredient);

                if (leftToRemove != null)
                    return false;
            }
            
            _sourceInventory.TryAddItem(recipe.Result);
            
            return true;
        }

        [Button]
        private void Craft()
        {
            var result = TryCraft(_testRecipe);
            
            Debug.Log($"Crafting result: {result} for item: {_testRecipe.Result.ItemData}");
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