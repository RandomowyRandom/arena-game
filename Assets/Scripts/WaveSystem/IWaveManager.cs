using ServiceLocator;

namespace WaveSystem
{
    public interface IWaveManager: IService
    {
        public void SetWave(Wave wave);
        
        public void StartWave();
    }
}