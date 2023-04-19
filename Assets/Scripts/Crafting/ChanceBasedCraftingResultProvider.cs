using System;
using System.Collections.Generic;
using System.Linq;
using Items;
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
        
        [Button]
        private void SetDefaultChances()
        {
            _possibleResults.Clear();
            var chances = new[] { 0.04f, 0.08f, 0.25f, 0.5f, 1f };

            foreach (var chance in chances)
            {
                _possibleResults.Add(new ChanceBasedCraftingResult
                {
                    Chance = chance,
                    Result = null
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