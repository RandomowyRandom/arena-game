using ServiceLocator;

namespace Player.Interfaces
{
    public interface IPlayerHotbarHandler: IService
    {
        public int CurrentDurability { get; }   
    }
}