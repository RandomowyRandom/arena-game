using Cysharp.Threading.Tasks;
using Items.ItemDataSystem;
using UnityEngine;

namespace Items.Abstraction
{
    public interface IItemUser
    {
        public GameObject GameObject { get; }
        public GameObject ParentGameObject { get; }
        public UniTask<bool> TryUseItem(UsableItem item);
        public void ConsumeItem(UsableItem item);
    }
}