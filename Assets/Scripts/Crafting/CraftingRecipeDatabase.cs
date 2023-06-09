﻿using System.Collections.Generic;
using System.Linq;
using Common.Attributes;
using Inventory.Interfaces;
using Player.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Crafting
{
    [ScriptableFactoryElement]
    public class CraftingRecipeDatabase: SerializedScriptableObject
    {
        [SerializeField]
        private List<CraftingRecipe> _recipes;

        public CraftingRecipe GetRecipe(string key)
        {
            return _recipes.FirstOrDefault(recipe => recipe.Key == key);
        }
        
        public List<CraftingRecipe> GetAvailableRecipes(IInventory inventory, bool excludeCraftable = false)
        {
            var itemsInInventory = inventory.Items
                .Where(item => item != null)
                .Select(i => i.ItemData).ToList();
            
            var recipes = new List<CraftingRecipe>();
            
            var craftableRecipes = GetCraftableRecipes(inventory);
            
            foreach (var recipe in _recipes)
            {
                var ingredients = recipe.Ingredients;
                
                var isRecipeAvailable = ingredients.Any(ingredient => itemsInInventory.Contains(ingredient.ItemData));

                if (!isRecipeAvailable)
                    continue;
                
                if(excludeCraftable && craftableRecipes.Contains(recipe))
                    continue;
                    
                recipes.Add(recipe);
            }
            
            return recipes.OrderByDescending(
                    recipe => recipe.Ingredients
                        .Sum(ingredient => ingredient.Amount))
                        .ToList();
        }

        public List<CraftingRecipe> GetCraftableRecipes(IInventory inventory)
        {
            var items = inventory.Items
                .Where(item => item != null);
            
            var recipes = new List<CraftingRecipe>();

            foreach (var recipe in _recipes)
            {
                var ingredients = recipe.Ingredients;
                
                var isRecipeAvailable = ingredients
                    .All(ingredient => items
                        .Any(i => i.ItemData == ingredient.ItemData && i.Amount >= ingredient.Amount));
                
                if (isRecipeAvailable)
                    recipes.Add(recipe);
            }

            return recipes.OrderByDescending(
                recipe => recipe.Ingredients
                    .Sum(ingredient => ingredient.Amount))
                    .ToList();
        }

        [InfoBox("Runtime tests", InfoMessageType.Warning)]
        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        private void TestAvailableRecipes()
        {
            var playerInventory = ServiceLocator.ServiceLocator.Instance.Get<IPlayerInventory>();
            
            var recipes = GetAvailableRecipes(playerInventory.Inventory);

            foreach (var recipe in recipes)
            {
                Debug.Log(recipe.name);
            }
        }
        
        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        private void TestCraftableRecipes()
        {
            var playerInventory = ServiceLocator.ServiceLocator.Instance.Get<IPlayerInventory>();
            
            var recipes = GetCraftableRecipes(playerInventory.Inventory);

            foreach (var recipe in recipes)
            {
                Debug.Log(recipe.name);
            }
        }
    }
}