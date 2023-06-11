using System.Collections.Generic;
using Common.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Items.RaritySystem
{
    [ScriptableFactoryElement]
    public class GearRarityDatabase: SerializedScriptableObject
    {
        [SerializeField]
        private List<GearRarity> _rarities;
        
        public List<GearRarity> Rarities => _rarities;
        
        public GearRarity GetRandomRarity()
        {
            var chances = new[] { 0.04f, 0.08f, 0.25f, 0.5f, 1f };

            for (var i = 0; i < chances.Length; i++)
            {
                var chance = chances[i];
                var rarity = _rarities[i];
                
                if (UnityEngine.Random.value <= chance)
                    return rarity;
            }
            
            return _rarities[0];
        }
    }
}