using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Items.Abstraction;
using Items.ItemDataSystem;
using UnityEngine;

namespace Items.Effects
{
    [Serializable]
    public class SetItemVisibilityVisualItemEffect: IItemEffect
    {
        [SerializeField]
        private bool _visible;
        
        [SerializeField]
        private float _scaleDuration = .15f;
        public UniTask OnUse(IItemUser user, UsableItem item)
        {
            user.GameObject.transform.DOScale(_visible ? Vector3.one : Vector3.zero, _scaleDuration);
            
            return UniTask.CompletedTask;
        }
    }
}