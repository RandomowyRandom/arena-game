using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Items.Abstraction;
using Items.ItemDataSystem;
using UnityEngine;

namespace Items.Effects
{
    [Serializable]
    public class MeleeScaleVisualItemEffect: IItemEffect
    {
        [SerializeField]
        private float _scaleDuration = .2f;
        
        [SerializeField]
        private float _rotationDuration = .15f;
        public UniTask OnUse(IItemUser user, UsableItem item)
        {
            user.GameObject.transform.localScale = Vector3.zero;
            user.GameObject.transform.DOScale(Vector3.one, _scaleDuration);
            var zRotation = user.GameObject.transform.rotation.eulerAngles.z;
            user.GameObject.transform.DORotate(new Vector3(0, 0,zRotation + 360), _rotationDuration, RotateMode.FastBeyond360);
            
            return UniTask.CompletedTask;
        }
    }
}