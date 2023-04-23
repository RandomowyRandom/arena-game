using System;
using Cysharp.Threading.Tasks;
using Items.Abstraction;
using Items.ItemDataSystem;
using UnityEngine;

namespace Items.Effects
{
    [Serializable]
    public class PushItemUserItemEffect: IItemEffect
    {
        [SerializeField]
        private float _force = 10;
        public UniTask OnUse(IItemUser user, UsableItem item)
        {
            var rigidbody = user.ParentGameObject.GetComponent<Rigidbody2D>();
            rigidbody.AddForce(user.GameObject.transform.right * _force, ForceMode2D.Impulse);
            
            return UniTask.CompletedTask;
        }
    }
}