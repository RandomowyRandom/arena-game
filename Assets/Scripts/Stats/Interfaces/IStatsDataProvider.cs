using Items.RaritySystem;

namespace Stats.Interfaces
{
    public interface IStatsDataProvider
    {
        public StatsData GetStatsData(GearRarity gearRarity);
    }
}