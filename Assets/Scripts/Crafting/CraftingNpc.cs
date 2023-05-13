using System;
using System.Linq;
using Common.Extensions;
using Crafting.Abstraction;
using Inventory.Interfaces;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Crafting
{
    public class CraftingNpc: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private ICraftingHandler _craftingHandler;
        
        [SerializeField]
        private CraftInteraction _craftInteractionPrefab;
        
        private IInventory _sourceInventory;

        private void Start()
        {
            _sourceInventory = ServiceLocator.ServiceLocator.Instance.Get<IPlayerInventory>().Inventory;
            InstantiateCraftInteractions();
        }

        [Button]
        private void InstantiateCraftInteractions()
        {
            DestroyCraftInteractions();
            
            var allRecipes = _craftingHandler.CraftingRecipeDatabase
                .GetAvailableRecipes(_sourceInventory);
            
            allRecipes.Shuffle();
            
            var recipes = allRecipes.Take(3).ToList();

            var y = -.7f;
            var x = -2f;
            
            foreach (var recipe in recipes)
            {
                var interaction = Instantiate(
                    _craftInteractionPrefab, transform.position + new Vector3(x, y), Quaternion.identity, 
                    transform);
                
                interaction.SetRecipe(recipe);

                x++;
            }
        } 
        
        private void DestroyCraftInteractions()
        {
            foreach (Transform child in transform)
            {
                if(transform.GetComponent<CraftInteraction> () == null)
                    return;
                
                Destroy(child.gameObject);
            }
        }
    }
}