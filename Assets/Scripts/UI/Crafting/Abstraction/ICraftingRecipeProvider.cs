using System;
using Crafting;

namespace UI.Crafting.Abstraction
{
    public interface ICraftingRecipeProvider
    {
        public event Action OnRecipeChanged; 
        
        public CraftingRecipe GetRecipe();
    }
}