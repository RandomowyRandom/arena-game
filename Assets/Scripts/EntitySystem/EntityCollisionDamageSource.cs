using System.Collections.Generic;
using System.Data;
using EntitySystem.Abstraction;
using UnityEngine;

namespace EntitySystem
{
    public class EntityCollisionDamageSource: MonoBehaviour, IDamageSource
    {
        [SerializeField]
        private float _collisionDamage = 1f;
        public GameObject Source
        {
            get => gameObject;
            set => throw new ReadOnlyException("Cannot set Source on EntityCollisionDamageSource");
        }

        public EntityAttackerGroup AttackerGroup
        {
            get => EntityAttackerGroup.Other;
            set => throw new ReadOnlyException("Cannot set AttackerGroup on EntityCollisionDamageSource");
        }
        public float Damage => _collisionDamage;
        public bool CanAttackEntity(Entity entity) => true;
        public void EntityAttacked(Entity entity) { }
    }
}