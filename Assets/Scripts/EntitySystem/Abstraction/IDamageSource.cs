using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem.Abstraction
{
    public interface IDamageSource
    {
        public GameObject Source { get; set; }
        public EntityAttackerGroup AttackerGroup { get; set; }
        public float Damage { get; }
        public bool CanAttackEntity(Entity entity);
        public void EntityAttacked(Entity entity);
    }
}