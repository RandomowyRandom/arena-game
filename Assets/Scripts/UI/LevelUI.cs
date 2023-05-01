using System;
using Player.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelUI: MonoBehaviour
    {
        [SerializeField]
        private Slider _valueBar;
        
        [SerializeField]
        private TMP_Text _levelText;
        
        private IPlayerLevel _playerLevel;
        
        private IPlayerLevel PlayerLevel => _playerLevel ??= _playerLevel = ServiceLocator.ServiceLocator.Instance.Get<IPlayerLevel>();
        
        
        private void Start()
        {
            PlayerLevel.OnLevelChanged += UpdateUI;
            
            SetLevelUI(PlayerLevel.CurrentLevel, PlayerLevel.CurrentExperience, PlayerLevel.NextLevelExperience);
        }

        private void OnDestroy()
        {
            PlayerLevel.OnLevelChanged -= UpdateUI;
        }

        private void SetLevelUI(int level, int currentExperience, int nextLevelExperience)
        {
            _valueBar.value = currentExperience;
            _valueBar.maxValue = nextLevelExperience;
            _levelText.text = $"Level: {level}";
        }
        
        private void UpdateUI()
        {
            SetLevelUI(PlayerLevel.CurrentLevel, PlayerLevel.CurrentExperience, PlayerLevel.NextLevelExperience);
        }
    }
}