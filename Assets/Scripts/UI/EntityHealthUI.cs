using EntitySystem;
using EntitySystem.Abstraction;
using PlayerUpgradeSystem.Abstraction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EntityHealthUI: MonoBehaviour
    {
        [SerializeField]
        private Slider _valueBar;
        
        [SerializeField]
        private TMP_Text _healthText;
        
        [SerializeField]
        private Entity _entity;
        
        private IPlayerUpgradeHandler _playerUpgradeHandler;
        
        private void Start()
        {
            _playerUpgradeHandler = ServiceLocator.ServiceLocator.Instance.Get<IPlayerUpgradeHandler>();
            
            _entity.OnDamageTaken += UpdateUI;
            _entity.OnHeal += amount => UpdateUI(amount, null);
            _playerUpgradeHandler.OnUpgradeChanged += () => SetHealthUI(_entity.Health, _entity.MaxHealth);
            
            SetHealthUI(_entity.Health, _entity.MaxHealth);
        }

        private void OnDestroy()
        {
            _entity.OnDamageTaken -= UpdateUI;
        }

        private void UpdateUI(float damage, IDamageSource source)
        {
            SetHealthUI(_entity.Health, _entity.MaxHealth);
        }
        
        private void SetHealthUI(float value, float maxValue)
        {
            if (_valueBar != null)
            {
                _valueBar.maxValue = maxValue;
                _valueBar.value = value;
            }
            
            if (_healthText != null)
            {
                _healthText.text = $"{Mathf.CeilToInt(value)}/{Mathf.CeilToInt(maxValue)}";
            }
        }
    }
}