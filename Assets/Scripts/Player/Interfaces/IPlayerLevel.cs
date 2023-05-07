using System;
using ServiceLocator;

namespace Player.Interfaces
{
    public interface IPlayerLevel: IService
    {
        public event Action OnLevelChanged;
        
        public int CurrentLevel { get; }
        
        public int CurrentExperience { get; }
        
        public int NextLevelExperience { get; }
        
        public void AddExperience(int experience);
    }
}