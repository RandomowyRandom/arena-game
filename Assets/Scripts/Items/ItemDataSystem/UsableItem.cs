using System.Collections.Generic;
using Common.Attributes;
using Cysharp.Threading.Tasks;
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
        
        public async UniTask OnUse(IItemUser user)
        {
            if(_effects == null)
                return;
            
            foreach (var effect in _effects)
            {
                await effect.OnUse(user, this);
            }
        }
    }
}