using System;
using Items;
using Sirenix.Serialization;
using UnityEngine;

namespace EntitySystem
{
    [Serializable]
    public class ItemDrop
    {
        [OdinSerialize]
        private Item _item;
        
        [OdinSerialize] [Range(0f, 1f)]
        private float _chance;

        public Item Item => _item;

        public float Chance => _chance;
    }
}