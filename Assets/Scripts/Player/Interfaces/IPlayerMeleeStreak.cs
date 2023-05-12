using ServiceLocator;

namespace Player.Interfaces
{
    public interface IPlayerMeleeStreak: IService
    {
        public float DamageMultiplier { get; }
        
        public float CurrentStreakTimer { get; }
    }
}