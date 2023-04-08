using Items.ItemDataSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Items.Abstraction
{
    public interface IItemUser
    {
        public GameObject GameObject { get; }
        public void UseItem(UsableItem item);
    }
}