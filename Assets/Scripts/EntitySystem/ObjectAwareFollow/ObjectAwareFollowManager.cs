using System;
using System.Collections.Generic;
using EntitySystem.ObjectAwareFollow.Abstraction;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace EntitySystem.ObjectAwareFollow
{
    public class ObjectAwareFollowManager: SerializedMonoBehaviour, IObjectAwareFollowManager
    {
        [OdinSerialize] [ReadOnly]
        private List<FollowWeightProvider> _providers = new();

        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IObjectAwareFollowManager>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IObjectAwareFollowManager>();
        }

        public void RegisterWeightProvider(FollowWeightProvider provider)
        {
            _providers.Add(provider);
        }

        public void DeregisterWeightProvider(FollowWeightProvider provider)
        {
            _providers.Remove(provider);
        }
        
        public Vector2 GetVectorForTransform(Transform providedTransform, float distanceTolerance = 2f)
        {
            var moveVector = Vector2.zero;
            
            var highestWeightProvider = GetHighestWeightProvider();
            
            moveVector = (highestWeightProvider.transform.position - providedTransform.position).normalized * highestWeightProvider.Weight;

            var distanceToHighestWeightProvider = Vector2.Distance(highestWeightProvider.transform.position, providedTransform.position);
            
            if(distanceToHighestWeightProvider < distanceTolerance)
                return moveVector.normalized;
            
            foreach (var provider in _providers)
            {
                if (provider == highestWeightProvider)
                    continue;
                
                var distance = Vector2.Distance(provider.transform.position, providedTransform.position);
                if (distance < 3f)
                {
                    moveVector += (Vector2)(provider.transform.position - providedTransform.position).normalized * provider.Weight;
                }
            }
            
            return moveVector.normalized;
        }

        private FollowWeightProvider GetHighestWeightProvider()
        {
            var highestWeightProvider = _providers[0];
            var highestWeight = highestWeightProvider.Weight;
            
            foreach (var provider in _providers)
            {
                if (provider.Weight > highestWeight)
                {
                    highestWeight = provider.Weight;
                    highestWeightProvider = provider;
                }
            }
            
            return highestWeightProvider;
        }
    }
}