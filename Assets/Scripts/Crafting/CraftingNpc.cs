using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Crafting.Abstraction;
using Items;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UI.Crafting.Abstraction;
using UnityEditor;
using UnityEngine;
using WaveSystem;

namespace Crafting
{
    public class CraftingNpc: SerializedMonoBehaviour, ICraftingRecipeProvider
    {
        [OdinSerialize]
        private ICraftingHandler _craftingHandler;
        
        [SerializeField]
        private CraftInteraction _craftInteractionPrefab;
        
        [OdinSerialize]
        private List<CraftingStationData> _craftingStationDatas;
        
        [SerializeField]
        private TMP_Text _tooltipText;
        
        [SerializeField]
        private SpriteRenderer _cloudRenderer;
        
        [SerializeField]
        private int _spawnInterval = 5;

        private IWaveManager _waveManager;
        public event Action OnRecipeChanged;

        private IPlayerLevel _playerLevel;
        private IPlayerInventory _playerInventory;

        private CraftInteraction _selectedInteraction;

        private void Start()
        {
            _waveManager = GetComponentInParent<IWaveManager>();
            _playerLevel = ServiceLocator.ServiceLocator.Instance.Get<IPlayerLevel>();
            _playerInventory = ServiceLocator.ServiceLocator.Instance.Get<IPlayerInventory>();
            InstantiateCraftInteractions();
            
            _waveManager.OnWaveStart += TryHideNpc;
            _waveManager.OnWaveEnd += TryShowNpc;
            
            TryHideNpc(null);
        }
        private void OnDestroy()
        {
            _waveManager.OnWaveStart -= TryHideNpc;
            _waveManager.OnWaveEnd -= TryShowNpc;
        }

        public CraftingRecipe GetRecipe()
        {
            return _selectedInteraction == null ? null : _selectedInteraction.GetRecipe();
        }


        private void TryShowNpc(Wave wave)
        {
            if(wave.Index % _spawnInterval != 0)
                return;
            
            gameObject.SetActive(true);
            InstantiateCraftInteractions();
            
            DeselectInteraction(null);
        }

        private void TryHideNpc(Wave wave)
        {
            gameObject.SetActive(false);
        }

        private void InstantiateCraftInteractions()
        {
            DestroyCraftInteractions();

            var y = -.9f;
            var x = -2f;
            
            foreach (var data in _craftingStationDatas)
            {
                var recipes = _craftingHandler.CraftingRecipeDatabase
                    .GetAvailableRecipes(_playerInventory.Inventory);
                
                recipes.Shuffle();
                
                var recipesToDisplay = recipes.Take(data.Amount).ToList();
                
                foreach (var recipe in recipesToDisplay)
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
            var itemToShow = new Item(resultItem.ItemData, resultItem.Amount);
            
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