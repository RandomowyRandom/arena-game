using Items.ItemDataSystem;

namespace Items.Abstraction
{
    public interface IItemEffect
    {
        void OnUse(IItemUser user, UsableItem item);
    }
}