using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using Sirenix.Serialization;
using UnityEngine;

namespace Crafting
{
    [Serializable]
    public class ChanceBasedCraftingResultProvider: ICraftingResultProvider
    {
        [OdinSerialize]
        private List<ChanceBasedCraftingResult> _possibleResults;
        
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
            
            public Item Result => _result;
            
            public float Chance => _chance;
        }
    }
}