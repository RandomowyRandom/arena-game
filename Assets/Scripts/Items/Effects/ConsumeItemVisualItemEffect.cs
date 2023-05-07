using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Items.Abstraction;
using Items.ItemDataSystem;
using UnityEngine;

namespace Items.Effects
{
    [Serializable]
    public class ConsumeItemVisualItemEffect: IItemEffect
    {
        public async UniTask OnUse(IItemUser user, UsableItem item)
        {
            user.GameObject.transform.DOLocalRotate(new Vector3(0, 0, 90), .2f);
            await UniTask.Delay(TimeSpan.FromSeconds(.1f));

            user.GameObject.transform.DOScale(Vector3.zero, .2f).OnComplete(() =>
            {
                user.GameObject.transform.rotation = Quaternion.identity;
                user.GameObject.transform.localScale = Vector3.one;
            });
        }
    }
}