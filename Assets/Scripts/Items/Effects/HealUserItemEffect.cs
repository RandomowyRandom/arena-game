using System;
using Cysharp.Threading.Tasks;
using EntitySystem;
using Items.Abstraction;
using Items.ItemDataSystem;
using UnityEngine;

namespace Items.Effects
{
    [Serializable]
    public class HealUserItemEffect: IItemEffect
    {
        [SerializeField]
        private float _healAmount;
        public UniTask OnUse(IItemUser user, UsableItem item)
        {
            var userHealth = user.ParentGameObject.GetComponent<Entity>();
            
            userHealth.Heal(_healAmount);
            
            return UniTask.CompletedTask;
        }
    }
}