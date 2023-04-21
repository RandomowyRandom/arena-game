using System.Collections.Generic;

namespace WaveSystem
{
    public class Wave
    {
        private List<SubWave> _subWaves;
        private float _subWaveDelay;
        
        public Wave(List<SubWave> subWaves, float subWaveDelay)
        {
            _subWaves = subWaves;
            _subWaveDelay = subWaveDelay;
        }
        
        public List<SubWave> SubWaves => _subWaves;
        public float SubWaveDelay => _subWaveDelay;
    }
}