using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Inventory.Interfaces;
using Sirenix.Serialization;

namespace Crafting.CraftingQueries
{
    [Serializable]
    public class AvailableWeaponsCraftingRecipeQuery: ICraftingRecipeQuery
    {
        [field: OdinSerialize]
        public int Amount { get; }
        public List<CraftingRecipe> GetRecipes(CraftingRecipeDatabase database, IInventory inventory)
        {
            var recipes = database.GetCraftableRecipesByObtainedWeapons(inventory);
            recipes.Shuffle();
            
            return recipes.Take(Amount).ToList();
        }
    }
}