namespace Inventory.Interfaces
{
    public interface IInventoryUI
    {
        public IInventory Inventory { get; }
        
        public void RegisterInventory(IInventory inventory);
        public void DeregisterInventory();
        public void UpdateUI();
    }
}