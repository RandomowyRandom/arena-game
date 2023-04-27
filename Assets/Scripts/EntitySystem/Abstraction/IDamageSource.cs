using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem.Abstraction
{
    public interface IDamageSource
    {
        public GameObject Source { get; set; }
        public List<IDamageable> DamagedEntities { get; }
        public float Damage { get; }
    }
}