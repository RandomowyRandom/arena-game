namespace Inventory.Interfaces
{
    public interface IInventoryUI
    {
        public void RegisterInventory(IInventory inventory);
        public void DeregisterInventory();
        public void UpdateUI();
    }
}