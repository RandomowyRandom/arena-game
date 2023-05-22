using System.Collections.Generic;
using System.Linq;

namespace WaveSystem
{
    public class Wave
    {
        private List<SubWave> _subWaves;
        private float _subWaveDelay;
        private int _enemyCap;
        private int _index;
        
        public Wave(List<SubWave> subWaves, float subWaveDelay, int enemyCap, int index)
        {
            _subWaves = subWaves;
            _subWaveDelay = subWaveDelay;
            _enemyCap = enemyCap;
            _index = index;
        }
        
        public List<SubWave> SubWaves => _subWaves;
        public float SubWaveDelay => _subWaveDelay;
        public int EnemyCap => _enemyCap;
        
        public int Index => _index;

        public override string ToString()
        {
            var enemies = new List<string>();
            
            foreach (var subWave in _subWaves)
            {
                enemies.AddRange(subWave.Entities.Select(entity => entity.name));
            }
            
            var enemiesString = string.Join(", ", enemies);
            
            return $"enemy cap: {_enemyCap}; Enemies: {enemiesString}";
        }
    }
}