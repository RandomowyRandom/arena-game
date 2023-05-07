using System;
using EntitySystem.Abstraction;
using LevelSystem;
using UnityEngine;

namespace EntitySystem
{
    [RequireComponent(typeof(Entity))]
    public class EntityExperienceDropper: MonoBehaviour
    {
        [SerializeField]
        private ExperienceOrb _experienceOrbPrefab;
        
        [SerializeField]
        private int _experienceOrbAmount;

        private Entity _entity;
        
        private void Awake()
        {
            _entity = GetComponent<Entity>();
            _entity.OnDeath += SpawnOrbs;
        }

        private void OnDestroy()
        {
            _entity.OnDeath -= SpawnOrbs;
        }

        private void SpawnOrbs(IDamageSource source)
        {
            for (var i = 0; i < _experienceOrbAmount; i++)
            {
                var experienceOrb = Instantiate(_experienceOrbPrefab, transform.position, Quaternion.identity);
                experienceOrb.transform.position += new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
            }
        }
    }
}