using System.Collections.Generic;
using Common.Attributes;
using Items.Abstraction;
using Sirenix.Serialization;
using UnityEngine;

namespace Items.ItemDataSystem
{
    [ScriptableFactoryElement]
    public class UsableItem: ItemData
    {
        [OdinSerialize] 
        private List<IItemEffect> _effects;
        
        [SerializeField]
        private bool _consumeOnUse;
        
        public bool ConsumeOnUse => _consumeOnUse;
        
        public void OnUse(IItemUser user)
        {
            foreach (var effect in _effects)
            {
                effect.OnUse(user, this);
            }
        }
    }
}