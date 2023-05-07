using System;
using ServiceLocator;

namespace WaveSystem
{
    public interface IWaveManager: IService
    {
        public event Action<Wave> OnWaveStart;
        public event Action<Wave> OnWaveEnd;
        public event Action<SubWave> OnSubWaveStart;
        public event Action<SubWave> OnSubWaveEnd;
        
        public bool IsWaveInProgress { get; }
        
        public void SetWave(Wave wave);
        
        public void StartWave();
    }
}