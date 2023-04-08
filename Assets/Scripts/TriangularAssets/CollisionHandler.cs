using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace TriangularAssets
{
    public class CollisionHandler : SerializedMonoBehaviour
    {
        [SerializeField] 
        private UnityEvent<GameObject> _onCollisionEnter;
        
        [SerializeField] 
        private UnityEvent<GameObject> _onCollisionExit;
        
        [SerializeField] 
        private UnityEvent<GameObject> _onCollisionStay;
        
        [OdinSerialize]
        private List<Type> _acceptedTypes = new();
        
        [Space]
        
        [SerializeField] private bool _includeTriggers = true;
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_acceptedTypes.Count > 0)
            {
                var hasAcceptedType = _acceptedTypes.Any(acceptedType => other.gameObject.GetComponent(acceptedType) != null);

                if (!hasAcceptedType)
                    return;
            }
            
            _onCollisionEnter.Invoke(other.gameObject);
        }
        
        private void OnCollisionExit2D(Collision2D other)
        {
            if (_acceptedTypes.Count > 0)
            {
                var hasAcceptedType = _acceptedTypes.Any(acceptedType => other.gameObject.GetComponent(acceptedType) != null);

                if (!hasAcceptedType)
                    return;
            }
            
            _onCollisionExit.Invoke(other.gameObject);
        }
        
        private void OnCollisionStay2D(Collision2D other)
        {
            if (_acceptedTypes.Count > 0)
            {
                var hasAcceptedType = _acceptedTypes.Any(acceptedType => other.gameObject.GetComponent(acceptedType) != null);

                if (!hasAcceptedType)
                    return;
            }
            
            _onCollisionStay.Invoke(other.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!_includeTriggers)
                return;
            
            if (_acceptedTypes.Count > 0)
            {
                var hasAcceptedType = _acceptedTypes.Any(acceptedType => other.gameObject.GetComponent(acceptedType) != null);

                if (!hasAcceptedType)
                    return;
            }
            
            _onCollisionEnter.Invoke(other.gameObject);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if(!_includeTriggers)
                return;
            
            if (_acceptedTypes.Count > 0)
            {
                var hasAcceptedType = _acceptedTypes.Any(acceptedType => other.gameObject.GetComponent(acceptedType) != null);

                if (!hasAcceptedType)
                    return;
            }
            
            _onCollisionExit.Invoke(other.gameObject);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if(!_includeTriggers)
                return;
            
            if (_acceptedTypes.Count > 0)
            {
                var hasAcceptedType = _acceptedTypes.Any(acceptedType => other.gameObject.GetComponent(acceptedType) != null);

                if (!hasAcceptedType)
                    return;
            }
            
            _onCollisionStay.Invoke(other.gameObject);
        }
    }
}