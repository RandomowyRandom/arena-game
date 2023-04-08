using UnityEngine;

namespace EntitySystem
{
    public interface IDamageSource
    {
        public GameObject Source { get; set; }
        public float Damage { get; }
    }
}