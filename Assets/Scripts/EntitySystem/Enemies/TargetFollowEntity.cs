using System;
using EntitySystem.Abstraction;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace EntitySystem.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Entity))]
    public class TargetFollowEntity: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private IEntityTargetProvider _targetProvider;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var target = _targetProvider.GetTarget();
            
            if (target == null)
                return;
            
            const float speed = 40f;
            
            var direction = (target.transform.position - transform.position).normalized;
            var movementVector = direction * speed;
            
            _rigidbody.AddForce(movementVector, ForceMode2D.Force);
        }
    }
}