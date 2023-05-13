using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Crafting.Abstraction;
using Inventory.Interfaces;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;
using WaveSystem;

namespace Crafting
{
    public class CraftingNpc: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private ICraftingHandler _craftingHandler;
        
        [SerializeField]
        private CraftInteraction _craftInteractionPrefab;
        
        [OdinSerialize]
        private List<CraftingStationData> _craftingStationDatas;

        private IPlayerLevel _playerLevel;

        private IWaveManager _waveManager;
        
        private void Start()
        {
            _playerLevel = ServiceLocator.ServiceLocator.Instance.Get<IPlayerLevel>();
            _waveManager = ServiceLocator.ServiceLocator.Instance.Get<IWaveManager>();
            InstantiateCraftInteractions();
            
            _waveManager.OnWaveStart += HideNpc;
            _waveManager.OnWaveEnd += ShowNpc;
            
            HideNpc(null);
        }

        private void OnDestroy()
        {
            _waveManager.OnWaveStart -= HideNpc;
            _waveManager.OnWaveEnd -= ShowNpc;
        }

        private void ShowNpc(Wave obj)
        {
            gameObject.SetActive(true);
            InstantiateCraftInteractions();
        }

        private void HideNpc(Wave obj)
        {
            gameObject.SetActive(false);
        }

        private void InstantiateCraftInteractions()
        {
            DestroyCraftInteractions();

            var y = -.7f;
            var x = -2f;
            
            foreach (var data in _craftingStationDatas)
            {
                var recipes = _craftingHandler.CraftingRecipeDatabase
                    .GetRecipesByTypeAndLevel(data.ItemType, _playerLevel.CurrentLevel - 2, _playerLevel.CurrentLevel);
                
                recipes.Shuffle();
                
                var recipesToDisplay = recipes.Take(data.Amount).ToList();
                
                foreach (var recipe in recipesToDisplay)
                {
                    var interaction = Instantiate(
                        _craftInteractionPrefab, transform.position + new Vector3(x, y), Quaternion.identity, 
                        transform);
                
                    interaction.CraftingHandler = _craftingHandler;
                    interaction.SetRecipe(recipe);

                    x++;
                }
                
                x++;
            }
        } 
        
        private void DestroyCraftInteractions()
        {
            foreach (Transform child in transform)
            {
                if(child.GetComponent<CraftInteraction>() == null)
                    continue;
                
                Destroy(child.gameObject);
            }
        }
    }
}