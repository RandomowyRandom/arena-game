using System.Collections.Generic;
using System.Linq;
using Common.Attributes;
using Items.Abstraction;
using Items.Durability;
using Items.RaritySystem;
using Sirenix.Serialization;
using Stats;
using Stats.Interfaces;
using UnityEngine;

namespace Items.ItemDataSystem
{
    [ScriptableFactoryElement]
    public class Weapon: UsableItem, IStatsDataProvider, IGearRaritiesProvider
    {
        [SerializeField]
        private int _maxDurability;
        
        [OdinSerialize]
        private List<GearRarityData> _rarityData;
        
        [Space(5)]
        [SerializeField]
        private GearRarityDatabase _gearRarityDatabase;
        public int Durability { get; set; }
        
        public void SetRarityData(List<GearRarityData> rarityData)
        {
            _rarityData = new(rarityData);
        }

        public override void OnItemConstructed(Item item)
        {
            var hasItemDurabilityData = item.HasAdditionalData<DurabilityItemData>();
            var hasItemRarityData = item.HasAdditionalData<RarityAdditionalItemData>();
            
            if(!hasItemRarityData)
                item.AddAdditionalData(new RarityAdditionalItemData(_gearRarityDatabase.GetRandomRarity()));
            
            if(!hasItemDurabilityData)
                item.AddAdditionalData(new DurabilityItemData(_maxDurability));
        }

        public StatsData GetStatsData(GearRarity rarity)
        {
            return  _rarityData
                .Where(r => r.GearRarity == rarity)
                .Select(r => r.StatsData).FirstOrDefault();
        }
        
        public List<GearRarityData> GetGearRarities()
        {
            return _rarityData;
        }
    }
}