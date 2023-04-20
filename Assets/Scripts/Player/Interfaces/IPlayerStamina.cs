using ServiceLocator;

namespace Player.Interfaces
{
    public interface IPlayerStamina: IService
    {
        public float CurrentStamina { get; }
        public void DrainStamina(float amount);
        public void RegenStamina(float amount);

        public bool HasEnoughStamina(float amount);
    }
}