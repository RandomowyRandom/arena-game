using System;
using Items.ItemDataSystem;

namespace Player.Interfaces
{
    public interface IUsableItemProvider
    {
        public event Action OnUsableItemChanged;
        public UsableItem GetUsableItem();
    }
}