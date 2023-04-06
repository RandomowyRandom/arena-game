using ServiceLocator;
using Stats;

namespace Player.Interfaces
{
    public interface IPlayerStats: IService
    {
        public void SetStats(StatsData statsData);
    }
}