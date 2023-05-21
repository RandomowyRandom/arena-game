using System.Collections.Generic;
using Common.Attributes;
using Items;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Crafting
{
    [ScriptableFactoryElement]
    public class CraftingRecipe: SerializedScriptableObject
    {
        [OdinSerialize]
        private List<Item> _ingredients;
        
        [OdinSerialize]
        private ICraftingResultProvider _resultProvider;
        
        [OdinSerialize]
        private int _requiredLevel;
        
        public List<Item> Ingredients
        {
            get => _ingredients;
            set => _ingredients = value;
        }

        public Item Result => _resultProvider.GetResult();
        
        public int RequiredLevel => _requiredLevel;
        
        public string Key => name;

        public void SetResultProvider(ICraftingResultProvider resultProvider)
        {
            _resultProvider = resultProvider;
        } 
        
        [InfoBox("Runtime only")]
        [Button]
        private void AddNeededItems()
        {
            if(Application.isPlaying == false)
                throw new System.Exception("This method is only for runtime use");
            
            var playerInventory = ServiceLocator.ServiceLocator.Instance.Get<IPlayerInventory>();

            foreach (var ingredient in _ingredients)
            {
                var res = playerInventory.Inventory.TryAddItem(ingredient);

                if (res != null)
                    Debug.LogError($"Could not add {ingredient} to inventory");
            }
        }
    }
}