using Inventory.Interfaces;
using ServiceLocator;

namespace Player.Interfaces
{
    public interface IPlayerInventory: IService
    {
        public IInventory Inventory { get; }
    }
}