using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace PlayerUpgradeSystem
{
    [Serializable]
    public class ExcludedUpgrade
    {
        public ExcludedUpgrade(PlayerUpgrade upgrade, int excludedFor = 2)
        {
            Upgrade = upgrade;
            ExcludedFor = excludedFor;
        }
        
        [field: OdinSerialize, ReadOnly] 
        public PlayerUpgrade Upgrade { get; }
        
        [field: OdinSerialize, ReadOnly]
        public int ExcludedFor { get; set; }
    }
}