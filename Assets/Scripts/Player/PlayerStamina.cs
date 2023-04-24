using System;
using Player.Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerStamina: MonoBehaviour, IPlayerStamina
    {
        [SerializeField]
        private float _regenAfterSeconds = .7f;
        
        [SerializeField]
        private float _maxStamina = 100f;
        
        [SerializeField]
        private float _staminaRegenerationSpeed = 10f;

        public event Action<float> OnStaminaChanged;
        
        private float _currentStamina;
        private float _staminaRegenerationTimer;
        
        public float CurrentStamina => _currentStamina;
        public float MaxStamina => _maxStamina;
        private bool ShouldRegenStamina => _staminaRegenerationTimer < 0f;

        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IPlayerStamina>(this);
        }

        private void Start()
        {
            _currentStamina = _maxStamina;
        }

        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerStamina>();
        }

        public void DrainStamina(float amount)
        {
            _currentStamina -= amount;
            _staminaRegenerationTimer = _regenAfterSeconds;
            
            OnStaminaChanged?.Invoke(_currentStamina);
        }
        
        public void RegenStamina(float amount)
        {
            _currentStamina += amount;
            _currentStamina = Mathf.Clamp(_currentStamina, 0f, _maxStamina);
            
            OnStaminaChanged?.Invoke(_currentStamina);
        }

        public bool HasEnoughStamina(float amount)
        {
            return _currentStamina >= amount;
        }

        private void Update()
        {
            if(ShouldRegenStamina)
                RegenStamina(Time.deltaTime * _staminaRegenerationSpeed);
            else
                _staminaRegenerationTimer -= Time.deltaTime;
        }
    }
}