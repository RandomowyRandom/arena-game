using System;
using Player.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerStaminaUI: MonoBehaviour
    {
        [SerializeField]
        private Slider _valueBar;
        
        private IPlayerStamina _playerStamina;
        
        private void Start()
        {
            _playerStamina = ServiceLocator.ServiceLocator.Instance.Get<IPlayerStamina>();

            _playerStamina.OnStaminaChanged += UpdateUI;
            
            SetStaminaUI(_playerStamina.CurrentStamina, _playerStamina.MaxStamina);
        }

        private void OnDestroy()
        {
            _playerStamina.OnStaminaChanged -= UpdateUI;
        }

        private void SetStaminaUI(float currentStamina, float maxStamina)
        {
            _valueBar.maxValue = maxStamina;
            _valueBar.value = currentStamina;
        }

        private void UpdateUI(float currentStamina)
        {
            SetStaminaUI(currentStamina, _playerStamina.MaxStamina);
        }
    }
}