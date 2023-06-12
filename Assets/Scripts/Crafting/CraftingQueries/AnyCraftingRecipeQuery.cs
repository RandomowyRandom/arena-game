using System;
using System.Collections.Generic;
using Inventory.Interfaces;
using Sirenix.Serialization;
using UnityEngine;

namespace Crafting.CraftingQueries
{
    [Serializable]
    public class AnyCraftingRecipeQuery: ICraftingRecipeQuery
    {
        [field: OdinSerialize]
        public int Amount { get; }
        
        public List<CraftingRecipe> GetRecipes(CraftingRecipeDatabase database, IInventory inventory)
        {
            return database.GetRandomRecipes(Amount);
        }
    }
}