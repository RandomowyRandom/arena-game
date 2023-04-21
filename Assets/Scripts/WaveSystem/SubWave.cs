using System.Collections.Generic;
using EntitySystem;

namespace WaveSystem
{
    public class SubWave
    {
        public SubWave(List<Entity> entities, float spawnDelay)
        {
            Entities = entities;
            SpawnDelay = spawnDelay;
        }

        public SubWave()
        {
            Entities = new ();
        }
        
        public List<Entity> Entities { get; set; }

        public float SpawnDelay { get; set; }
    }
}