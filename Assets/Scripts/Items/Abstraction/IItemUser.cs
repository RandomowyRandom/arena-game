using Cysharp.Threading.Tasks;
using Items.ItemDataSystem;
using UnityEngine;

namespace Items.Abstraction
{
    public interface IItemUser
    {
        public GameObject GameObject { get; }
        public UniTask<bool> TryUseItem(UsableItem item);
    }
}