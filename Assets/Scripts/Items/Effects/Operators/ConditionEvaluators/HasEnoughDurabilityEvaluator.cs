using System;
using Items.Effects.Abstraction;
using Player.Interfaces;
using UnityEngine;

namespace Items.Effects.Operators.ConditionEvaluators
{
    [Serializable]
    public class HasEnoughDurabilityEvaluator: IConditionEvaluator
    {
        [SerializeField]
        private int _biggerThan = 0;
        
        private IPlayerHotbarHandler _playerHotbarHandler;
        
        private IPlayerHotbarHandler PlayerHotbarHandler => _playerHotbarHandler ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayerHotbarHandler>();
        public bool EvaluateCondition()
        {
            return PlayerHotbarHandler.CurrentDurability > _biggerThan;
        }
    }
}