using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using Items.ItemDataSystem;
using Items.RaritySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Crafting
{
    [Serializable]
    public class ChanceBasedCraftingResultProvider: ICraftingResultProvider
    {
        [OdinSerialize]
        private List<ChanceBasedCraftingResult> _possibleResults;
        
        public void SetDefaultChances(List<GearRarity> rarities, ItemData item)
        {
            _possibleResults = new();
            var chances = new[] { 0.04f, 0.08f, 0.25f, 0.5f, 1f };

            for (var i = 0; i < chances.Length; i++)
            {
                var chance = chances[i];
                var rarity = rarities[i];
                
                _possibleResults.Add(new ChanceBasedCraftingResult
                {
                    Chance = chance,
                    Result = new Item(item, 1, rarity)
                });
            }
        }
        
        public Item GetResult()
        {
            var random = UnityEngine.Random.value;
            
            _possibleResults.Sort((a, b) => a.Chance.CompareTo(b.Chance));

            return _possibleResults
                .Where(possibleResult => random <= possibleResult.Chance)
                .Select(possibleResult => possibleResult.Result)
                .FirstOrDefault();
        }
        private class ChanceBasedCraftingResult
        {
            [OdinSerialize]
            private Item _result;
            
            [OdinSerialize] [Range(0f, 1f)]
            private float _chance;
            
            public Item Result
            {
                get => _result;
                set => _result = value;
            }

            public float Chance
            {
                get => _chance;
                set => _chance = value;
            }
        }
    }
}