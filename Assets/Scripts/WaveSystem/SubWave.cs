using System.Collections.Generic;
using EntitySystem;

namespace WaveSystem
{
    public class SubWave
    {
        private readonly List<Entity> _entities;
        private readonly float _spawnDelay;
        
        public SubWave(List<Entity> entities, float spawnDelay)
        {
            _entities = entities;
            _spawnDelay = spawnDelay;
        }
        
        public List<Entity> Entities => _entities;
        public float SpawnDelay => _spawnDelay;
    }
}