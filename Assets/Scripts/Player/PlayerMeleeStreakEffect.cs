using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMeleeStreak))]
    public class PlayerMeleeStreakEffect: MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _multiplierText;
        
        private PlayerMeleeStreak _playerMeleeStreak;
        
        private void Awake()
        {
            _playerMeleeStreak = GetComponent<PlayerMeleeStreak>();
        }

        private void Start()
        {
            _playerMeleeStreak.OnMeleeStreakChanged += UpdateUI;
        }

        private void OnDestroy()
        {
            _playerMeleeStreak.OnMeleeStreakChanged -= UpdateUI;
        }

        private void UpdateUI()
        {
            _multiplierText.transform.DOKill();
            
            if (_playerMeleeStreak.DamageMultiplier == 1)
            {
                _multiplierText.text = string.Empty;
                return;
            }
            
            _multiplierText.transform.localScale = Vector3.one;
            _multiplierText.text = $"x{_playerMeleeStreak.DamageMultiplier:F1}";
            _multiplierText.transform.DOPunchScale(Vector3.one * 1.2f, 0.2f).onComplete += () =>
            {
                _multiplierText.transform.DOScale(Vector3.zero, _playerMeleeStreak.CurrentStreakTimer).SetEase(Ease.InQuart);
            };
            
            _multiplierText.color = _playerMeleeStreak.DamageMultiplier >= 2f ? Color.red : Color.white;
        }
    }
}