using System;
using System.Globalization;
using BuffSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuffUI: MonoBehaviour
    {
        [SerializeField]
        private Image _icon;
        
        [SerializeField]
        private TMP_Text _timeLeftText;
        
        private Buff _buff;
        
        public void SetBuff(Buff buff)
        {
            _buff = buff;
            _icon.sprite = buff.BuffData.Icon;
            
            SetTimeLeft();
        }

        private void Update()
        {
            SetTimeLeft();
        }

        private void SetTimeLeft()
        {
            var timeInSeconds = Mathf.CeilToInt(_buff.RemainingTime);
            _timeLeftText.text = $"{timeInSeconds}s";
        }
    }
}