using System;
using System.Collections.Generic;
using Crafting;
using Crafting.Abstraction;
using Inventory.Interfaces;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UI.Crafting.Abstraction;
using UnityEngine;

namespace UI.Crafting
{
    public class CraftingRecipesUI: SerializedMonoBehaviour, ICraftingRecipeProvider
    {
        [SerializeField]
        private CraftingRecipePreview _recipeSlotPrefab;
        
        [SerializeField]
        private Transform _recipeSlotsPanel;
        
        [OdinSerialize]
        private ICraftingHandler _craftingHandler;
        
        public event Action OnRecipeChanged;

        private readonly List<CraftingRecipePreview> _recipeSlots = new();

        private IInventory _playerInventory;
        
        private CraftingRecipe _selectedRecipe;
        
        private void Start()
        {
            _playerInventory = ServiceLocator.ServiceLocator.Instance.Get<IPlayerInventory>().Inventory;
         
            SetCraftingHandler(_craftingHandler);
            
            _playerInventory.OnInventoryChanged += RefreshRecipes;
            _playerInventory.OnInventoryChanged += OnRecipeChanged;
        }
        
        private void OnDestroy()
        {
            _playerInventory.OnInventoryChanged -= RefreshRecipes;
            _playerInventory.OnInventoryChanged -= OnRecipeChanged;
        }

        public void SetCraftingHandler(ICraftingHandler craftingHandler)
        {
            _craftingHandler = craftingHandler;
            
            RefreshRecipes();
        }
        
        private void RefreshRecipes()
        {
            foreach (var slot in _recipeSlots)
            {
                slot.OnRecipeSelected -= SelectRecipe;
                Destroy(slot.gameObject);
            }
            
            _recipeSlots.Clear();
            
            if (_craftingHandler == null)
                return;
            
            var availableRecipes = _craftingHandler.CraftingRecipeDatabase.GetAvailableRecipes(_playerInventory, true);
            var craftableRecipes = _craftingHandler.CraftingRecipeDatabase.GetCraftableRecipes(_playerInventory);
            
            foreach (var recipe in craftableRecipes)
            {
                var newSlot = Instantiate(_recipeSlotPrefab, _recipeSlotsPanel);
                newSlot.SetRecipe(recipe, true);
                newSlot.OnRecipeSelected += SelectRecipe;
                
                _recipeSlots.Add(newSlot);
            }
            
            foreach (var recipe in availableRecipes)
            {
                var newSlot = Instantiate(_recipeSlotPrefab, _recipeSlotsPanel);
                newSlot.SetRecipe(recipe, false);
                newSlot.OnRecipeSelected += SelectRecipe;
                
                _recipeSlots.Add(newSlot);
            }
        }

        private void SelectRecipe(CraftingRecipe recipe)
        {
            _selectedRecipe = recipe;
            OnRecipeChanged?.Invoke();
        }

        public CraftingRecipe GetRecipe()
        {
            return _selectedRecipe;
        }
    }
}