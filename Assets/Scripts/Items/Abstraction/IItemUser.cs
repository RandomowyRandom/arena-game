using Items.ItemDataSystem;
using UnityEngine;

namespace Items.Abstraction
{
    public interface IItemUser
    {
        public GameObject GameObject { get; }
        public bool TryUseItem(UsableItem item);
    }
}