using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EntitySystem
{
    [RequireComponent(typeof(Entity))]
    public class EntityDamageIndicatorSpawner: MonoBehaviour
    {
        [SerializeField] 
        private DamageIndicator _damageIndicator;
        
        private Entity _entity;
        
        private void Awake()
        {
            _entity = GetComponent<Entity>();
            _entity.OnDamageTaken += SpawnDamageIndicator;
        }

        private void OnDestroy()
        {
            _entity.OnDamageTaken -= SpawnDamageIndicator;
        }

        private void SpawnDamageIndicator(float damage)
        {
            var indicator = Instantiate(_damageIndicator, transform.position, Quaternion.identity);
            indicator.SetDamage(damage);
        }
    }
}