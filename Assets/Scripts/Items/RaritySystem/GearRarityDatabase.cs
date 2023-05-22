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
    }
}