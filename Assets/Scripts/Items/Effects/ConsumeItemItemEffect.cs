using System;
using Cysharp.Threading.Tasks;
using Items.Abstraction;
using Items.ItemDataSystem;

namespace Items.Effects
{
    [Serializable]
    public class ConsumeItemItemEffect: IItemEffect
    {
        public UniTask OnUse(IItemUser user, UsableItem item)
        {
            user.ConsumeItem(item);
            
            return UniTask.CompletedTask;
        }
    }
}