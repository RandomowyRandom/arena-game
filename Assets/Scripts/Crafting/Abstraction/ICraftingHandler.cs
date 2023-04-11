namespace Crafting.Abstraction
{
    public interface ICraftingHandler
    {
        public CraftingRecipeDatabase CraftingRecipeDatabase { get; }
        public bool CanCraft(CraftingRecipe recipe);
        
        public bool TryCraft(CraftingRecipe recipe);
    }
}