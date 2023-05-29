using System.Collections.Generic;
using Common.Attributes;
using Items.Abstraction;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Stats;
using UnityEngine;

namespace WeaponTemplate
{
    [ScriptableFactoryElement]
    public class WeaponTemplate: SerializedScriptableObject
    {
        [SerializeField]
        private Sprite _icon;
        
        [SerializeField]
        private string _name;

        [SerializeField]
        private string _description;

        [SerializeField]
        private int _requiredLevel;
        
        [SerializeField]
        private int _durability;
        
        [Space(10)]
        [OdinSerialize]
        private List<IItemEffect> _itemEffects;
        
        [Space(10)]
        [OdinSerialize]
        private StatsData _commonStatsData;

        public string Name => _name;

        public string Description => _description;

        public int RequiredLevel => _requiredLevel;

        public int Durability => _durability;
        
        public List<IItemEffect> ItemEffects => new(_itemEffects);

        public StatsData CommonStatsData => _commonStatsData;
        
        public Sprite Icon => _icon;
    }
}