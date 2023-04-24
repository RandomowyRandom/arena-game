using System.Collections.Generic;
using Items.RaritySystem;

namespace Items.Abstraction
{
    public interface IGearRaritiesProvider
    {
        public List<GearRarityData> GetGearRarities();
    }
}