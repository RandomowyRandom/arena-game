using System.Collections.Generic;
using System.Linq;
using Common.Attributes;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace PlayerUpgradeSystem
{
    [ScriptableFactoryElement]
    public class UpgradeDatabase: SerializedScriptableObject
    {
        [OdinSerialize]
        private List<PlayerUpgrade> _upgrades;
        
        [OdinSerialize] [ReadOnly]
        private List<ExcludedUpgrade> _temporaryExcludedUpgrades = new();

        protected override void OnAfterDeserialize()
        {
            _temporaryExcludedUpgrades = new List<ExcludedUpgrade>();
        }

        public List<PlayerUpgrade> GetUpgrades()
        {
            return new List<PlayerUpgrade>(_upgrades
                .Except(_temporaryExcludedUpgrades
                    .Select(x => x.Upgrade)));  
        }

        public void ExcludeUpgrade(PlayerUpgrade upgrade)
        {
            _temporaryExcludedUpgrades.Add(new ExcludedUpgrade(upgrade));
        }
        
        public void UpdateExclusions()
        {
            _temporaryExcludedUpgrades.ForEach(x => x.ExcludedFor--);
            _temporaryExcludedUpgrades.RemoveAll(x => x.ExcludedFor <= 0);
        }
    }
}