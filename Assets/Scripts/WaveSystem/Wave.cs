using System.Collections.Generic;
using System.Linq;

namespace WaveSystem
{
    public class Wave
    {
        private List<SubWave> _subWaves;
        private float _subWaveDelay;
        private int _enemyCap;
        
        public Wave(List<SubWave> subWaves, float subWaveDelay, int enemyCap)
        {
            _subWaves = subWaves;
            _subWaveDelay = subWaveDelay;
            _enemyCap = enemyCap;
        }
        
        public List<SubWave> SubWaves => _subWaves;
        public float SubWaveDelay => _subWaveDelay;
        public int EnemyCap => _enemyCap;

        public override string ToString()
        {
            var enemies = new List<string>();
            
            foreach (var subWave in _subWaves)
            {
                enemies.AddRange(subWave.Entities.Select(entity => entity.name));
            }
            
            var enemiesString = string.Join(", ", enemies);
            
            return $"Sub waves count: {_subWaves.Count}, Sub wave delay: {_subWaveDelay}; Enemies: {enemiesString}";
        }
    }
}