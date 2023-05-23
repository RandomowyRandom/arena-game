using System;
using ServiceLocator;

namespace PlayerUpgradeSystem.Abstraction
{
    public interface IPlayerUpgradeHandler: IService
    {
        public event Action OnEffectChanged;
        public void AddUpgrade(PlayerUpgrade playerUpgrade);

        public void RemoveUpgrade(PlayerUpgrade playerUpgrade);
    }
}