using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Items.Abstraction;
using Items.ItemDataSystem;
using UnityEngine;

namespace Items.Effects
{
    public class MeleeReappearVisualItemEffect: IItemEffect
    {
        public async UniTask OnUse(IItemUser user, UsableItem item)
        {
            user.GameObject.transform.localScale = Vector3.zero;
            await UniTask.Delay(TimeSpan.FromSeconds(.05f));
            user.GameObject.transform.DOScale(Vector3.one, .1f);
        }
    }
}