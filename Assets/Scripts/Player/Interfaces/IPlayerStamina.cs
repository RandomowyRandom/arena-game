using System;
using ServiceLocator;

namespace Player.Interfaces
{
    public interface IPlayerStamina: IService
    {
        public event Action<float> OnStaminaChanged;
        
        public float CurrentStamina { get; }
        
        public float MaxStamina { get; }
        public void DrainStamina(float amount);
        public void RegenStamina(float amount);

        public bool HasEnoughStamina(float amount);
    }
}