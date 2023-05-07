using System;
using BuffSystem;
using BuffSystem.Abstraction;
using Cysharp.Threading.Tasks;
using Items.Abstraction;
using Items.ItemDataSystem;
using Sirenix.Serialization;
using UnityEngine;

namespace Items.Effects
{
    [Serializable]
    public class GrantBuffItemEffect: IItemEffect
    {
        [OdinSerialize]
        private BuffData _buffData;
        
        private IBuffHandler _buffHandler;
        private IBuffHandler BuffHandler => _buffHandler 
            ??= ServiceLocator.ServiceLocator.Instance.Get<IBuffHandler>();
        
        public UniTask OnUse(IItemUser user, UsableItem item)
        {
            BuffHandler.AddBuff(_buffData);
            
            return UniTask.CompletedTask;
        }
    }
}