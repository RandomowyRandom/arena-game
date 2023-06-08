using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Items.Abstraction;
using Items.Effects.Abstraction;
using Items.ItemDataSystem;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEngine;

namespace Items.Effects.Operators
{
    [Serializable]
    public class ConditionalStatement: IItemEffect
    {
        [OdinSerialize]
        private IConditionEvaluator _conditionEvaluator;
        
        [SerializeField]
        private bool _expectedResult;
        
        [OdinSerialize]
        private List<IItemEffect> _effects;

        public ConditionalStatement(){}
        
        public ConditionalStatement(IConditionEvaluator conditionEvaluator, bool expectedResult, List<IItemEffect> effects)
        {
            _conditionEvaluator = conditionEvaluator;
            _expectedResult = expectedResult;
            _effects = effects;
        }
        
        public async UniTask OnUse(IItemUser user, UsableItem item)
        {
            var conditionResult = _conditionEvaluator.EvaluateCondition();

            if (_expectedResult != conditionResult)
                return;

            foreach (var effect in _effects)
            {
                await effect.OnUse(user, item);
            }
        }
    }
}