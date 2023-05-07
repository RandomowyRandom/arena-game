using Cysharp.Threading.Tasks;
using DG.Tweening;
using Items.Abstraction;
using Items.ItemDataSystem;
using UnityEngine;

namespace Items.Effects
{
    public class RangedSquashVisualItemEffect: IItemEffect
    {
        public UniTask OnUse(IItemUser user, UsableItem item)
        {
            user.GameObject.transform.DOScale(new Vector2(1.8f, 0.3f), .07f).OnComplete(
                () => user.GameObject.transform.DOScale(Vector3.one, .1f));
            
            return UniTask.CompletedTask;
        }
    }
}