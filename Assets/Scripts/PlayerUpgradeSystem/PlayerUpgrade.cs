using System.Collections.Generic;
using Common.Attributes;
using PlayerUpgradeSystem.Abstraction;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace PlayerUpgradeSystem
{
    [ScriptableFactoryElement]
    public class PlayerUpgrade: SerializedScriptableObject
    {
        [OdinSerialize]
        private List<IPlayerUpgradeEffect> _playerUpgradeEffects;
     
        [SerializeField]
        private Sprite _icon;
        
        [SerializeField]
        private string _description;
        
        public List<IPlayerUpgradeEffect> PlayerUpgradeEffects => _playerUpgradeEffects;
        
        public Sprite Icon => _icon;
        
        public string Description => _description;
    }
}