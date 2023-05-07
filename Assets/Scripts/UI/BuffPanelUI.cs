using System;
using BuffSystem.Abstraction;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class BuffPanelUI: MonoBehaviour
    {
        [SerializeField]
        private RectTransform _buffPanel;
        
        [SerializeField]
        private BuffUI _buffUIPrefab;
        
        private IBuffHandler _buffHandler;
        
        private IBuffHandler BuffHandler => _buffHandler ??= ServiceLocator.ServiceLocator.Instance.Get<IBuffHandler>();

        private void Start()
        {
            BuffHandler.OnBuffChanged += UpdateBuffs;
        }
        
        private void OnDestroy()
        {
            BuffHandler.OnBuffChanged -= UpdateBuffs;
        }

        public void UpdateBuffs()
        {
            foreach (Transform child in _buffPanel)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var buff in BuffHandler.GetBuffs())
            {
                var newBuffUI = Instantiate(_buffUIPrefab, _buffPanel);
                newBuffUI.SetBuff(buff);
            }
        }
    }
}