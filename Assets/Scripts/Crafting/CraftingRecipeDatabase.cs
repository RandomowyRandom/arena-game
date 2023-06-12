using System;
using System.Collections.Generic;
using System.Linq;
using Common.Attributes;
using Common.Extensions;
using Inventory.Interfaces;
using Items.ItemDataSystem;
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

        public List<CraftingRecipe> GetCraftableRecipesByObtainedWeapons(IInventory inventory)
        {
            var items = inventory.Items
                .Where(item => item != null).ToList();
            
            var recipes = new List<CraftingRecipe>();
            
            var weapons = items.Where(item => item.ItemData is Weapon).ToList();
            
            foreach (var recipe in _recipes)
            {
                var ingredients = recipe.Ingredients;
                var weaponInRecipe = ingredients.FirstOrDefault(ingredient => ingredient.ItemData is Weapon);
                
                if(weaponInRecipe == null)
                    continue;
                
                var doesPlayerHaveWeapon = weapons.Any(weapon => weapon.ItemData == weaponInRecipe.ItemData);
                
                if(doesPlayerHaveWeapon)
                    recipes.Add(recipe);
            }

            return recipes;
        }
        
        public List<CraftingRecipe> GetRecipesByTypeAndLevel(Type type, int minLevel, int maxLevel)
        {
            return _recipes
                .Where(recipe => recipe.Result.ItemData.GetType() == type 
                                 && recipe.RequiredLevel >= minLevel 
                                 && recipe.RequiredLevel <= maxLevel)
                .ToList();
        }
        
        public List<CraftingRecipe> GetRecipesForLevel(int level)
        {
            return _recipes.Where(recipe => recipe.RequiredLevel == level).ToList();
        }

        public List<CraftingRecipe> GetRandomRecipes(int amount)
        {
            var recipes = new List<CraftingRecipe>();

            for (var i = 0; i < amount; i++)
            {
                var recipe = _recipes.GetRandomElement();

                while (recipes.Contains(recipe))
                    recipe = _recipes.GetRandomElement();
                
                recipes.Add(recipe);
            }
            
            return recipes;
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