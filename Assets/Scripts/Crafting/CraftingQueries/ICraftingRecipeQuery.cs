using System.Collections.Generic;
using Inventory.Interfaces;

namespace Crafting.CraftingQueries
{
    public interface ICraftingRecipeQuery
    {
        public int Amount { get; }
        
        public List<CraftingRecipe> GetRecipes(CraftingRecipeDatabase database, IInventory inventory);
    }
}