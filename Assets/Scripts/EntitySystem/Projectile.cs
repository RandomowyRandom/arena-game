using UnityEngine;

namespace EntitySystem
{
    public class Projectile: MonoBehaviour, IDamageSource
    {
        [SerializeField] 
        private float _damage;

        public float Damage => _damage;
    }
}