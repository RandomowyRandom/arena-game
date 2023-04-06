namespace Items.Abstraction
{
    public interface IItemEffect
    {
        void OnUse(IItemUser user);
    }
}