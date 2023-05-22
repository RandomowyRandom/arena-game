using System;
using EntitySystem;
using PlayerUpgradeSystem.Abstraction;
using UnityEngine;

namespace PlayerUpgradeSystem.Effects
{
    [Serializable]
    public class HealPlayerUpgradeEffect: IPlayerUpgradeEffect
    {
        [SerializeField]
        private float _amount;
        public void OnObtain(PlayerUpgradeHandler playerUpgradeHandler)
        {
            playerUpgradeHandler.GetComponent<Entity>().Heal(_amount);
        }

        public void OnRemove(PlayerUpgradeHandler playerUpgradeHandler) { }
    }
}