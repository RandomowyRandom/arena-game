namespace PlayerUpgradeSystem.Abstraction
{
    public interface IPlayerUpgradeEffect
    {
        public void OnObtain(PlayerUpgradeHandler playerUpgradeHandler);
        
        public void OnRemove(PlayerUpgradeHandler playerUpgradeHandler);
    }
}