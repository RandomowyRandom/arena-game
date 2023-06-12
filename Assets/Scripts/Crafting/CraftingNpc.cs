using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Crafting.Abstraction;
using Crafting.CraftingQueries;
using Items;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UI.Crafting.Abstraction;
using UnityEngine;

namespace Crafting
{
    public class CraftingNpc: SerializedMonoBehaviour, ICraftingRecipeProvider
    {
        [OdinSerialize]
        private ICraftingHandler _craftingHandler;
        
        [SerializeField]
        private CraftInteraction _craftInteractionPrefab;
        
        [SerializeField]
        private TMP_Text _tooltipText;
        
        [SerializeField]
        private SpriteRenderer _cloudRenderer;
        
        [OdinSerialize]
        private ICraftingRecipeQuery _craftingRecipeQuery;
        public event Action OnRecipeChanged;

        private IPlayerInventory _playerInventory;

        private CraftInteraction _selectedInteraction;

        private void OnEnable()
        {
            _playerInventory = ServiceLocator.ServiceLocator.Instance.Get<IPlayerInventory>();

            if(_playerInventory == null)
                return;
            
            InstantiateCraftInteractions();
        }

        public CraftingRecipe GetRecipe()
        {
            return _selectedInteraction == null ? null : _selectedInteraction.GetRecipe();
        }
        

        private void InstantiateCraftInteractions()
        {
            DestroyCraftInteractions();

            var y = -.9f;
            var x = -2f;
            
            var recipes = _craftingRecipeQuery.GetRecipes(_craftingHandler.CraftingRecipeDatabase, _playerInventory.Inventory);
            
            if(recipes.Count == 0)
                return;
            
            recipes.Shuffle();
                
            foreach (var recipe in recipes)
            {
                var interaction = Instantiate(
                    _craftInteractionPrefab, transform.position + new Vector3(x, y), Quaternion.identity, 
                    transform);
                
                interaction.OnSelected += SelectInteraction;
                interaction.OnDeselected += DeselectInteraction;
                interaction.CraftingHandler = _craftingHandler;
                interaction.SetRecipe(recipe);

                x++;
            }
        }

        private void DeselectInteraction(CraftInteraction craftInteraction)
        {
            _selectedInteraction = null;
            _tooltipText.text = "";
            _cloudRenderer.enabled = false;
            
            OnRecipeChanged?.Invoke();
        }

        private void SelectInteraction(CraftInteraction craftInteraction)
        {
            _selectedInteraction = craftInteraction;
            var resultItem = _selectedInteraction.GetRecipe().Result;
            var itemToShow = new Item(resultItem);
            
            _tooltipText.text = itemToShow.GetTooltip();
            _cloudRenderer.enabled = true;
            
            OnRecipeChanged?.Invoke();
        }

        private void DestroyCraftInteractions()
        {
            foreach (Transform child in transform)
            {
                var craftInteraction = child.GetComponent<CraftInteraction>();
                if(craftInteraction == null)
                    continue;
                
                craftInteraction.OnSelected -= SelectInteraction;
                
                Destroy(child.gameObject);
            }
        }
    }
}