using System.Collections.Generic;
using Crafting;
using Items;
using Items.ItemDataSystem;
using Items.RaritySystem;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class CraftingRecipeFactory: OdinEditorWindow
    {
        [MenuItem("Redray/Crafting Recipe Factory")]
        private static void OpenWindow()
        {
            GetWindow<CraftingRecipeFactory>().Show();
        }
        
        [ShowInInspector]
        private GearRarityDatabase _gearRarityDatabase;
        [Space(10)]
        
        [ShowInInspector]
        private List<Item> _ingredients;
        
        [Space(50)]
        
        [ShowInInspector]
        private bool _createRarityData;
        
        [ShowInInspector]
        private ItemData _result;
        
        [Button(ButtonSizes.Large)]
        private void Create()
        {
            var recipe = CreateInstance<CraftingRecipe>();
            recipe.Ingredients = new(_ingredients);
            ICraftingResultProvider resultProvider = 
                _createRarityData ? new ChanceBasedCraftingResultProvider() : new BasicCraftingResultProvider();

            switch (resultProvider)
            {
                case ChanceBasedCraftingResultProvider chanceBasedCraftingResultProvider:
                    chanceBasedCraftingResultProvider.SetDefaultChances(_gearRarityDatabase.Rarities, _result);
                    break;
                case BasicCraftingResultProvider basicCraftingResultProvider:
                    basicCraftingResultProvider.SetResult(new Item(_result, 1));
                    break;
            }
            
            recipe.SetResultProvider(resultProvider);
            
            AssetDatabase.CreateAsset(recipe, "Assets/Scriptables/CraftingRecipe/" + _result.Key + ".asset");
            AssetDatabase.SaveAssets();
        }
    }
}