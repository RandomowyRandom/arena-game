using System;
using Sirenix.Serialization;
using UnityEngine;

namespace Crafting
{
    [Serializable]
    public class CraftingStationData
    {
        [OdinSerialize]
        private Type _itemType;
        
        [OdinSerialize]
        private int _amount;
        
        public Type ItemType => _itemType;
        public int Amount => _amount;
    }
}