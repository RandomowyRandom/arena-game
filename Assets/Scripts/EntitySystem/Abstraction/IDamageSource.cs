using UnityEngine;

namespace EntitySystem.Abstraction
{
    public interface IDamageSource
    {
        public GameObject Source { get; set; }
        public float Damage { get; }
    }
}