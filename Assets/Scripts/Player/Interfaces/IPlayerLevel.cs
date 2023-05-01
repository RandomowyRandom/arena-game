using ServiceLocator;

namespace Player.Interfaces
{
    public interface IPlayerLevel: IService
    {
        public int CurrentLevel { get; }
        
        public int CurrentExperience { get; }
        
        public void AddExperience(int experience);
    }
}