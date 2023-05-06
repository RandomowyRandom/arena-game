using System;
using EntitySystem.ObjectAwareFollow.Abstraction;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EntitySystem.ObjectAwareFollow
{
    public class FollowWeightProvider: SerializedMonoBehaviour
    {
        [SerializeField] [Range(-1f, 1f)]
        private float _weight = 0f;
        
        public float Weight => _weight;

        private IObjectAwareFollowManager _followManager;
        
        private IObjectAwareFollowManager FollowManager => 
            _followManager ??= 
                ServiceLocator.ServiceLocator.Instance.Get<IObjectAwareFollowManager>();
        
        private void Start()
        {
            FollowManager.RegisterWeightProvider(this);
        }
        
        private void OnDestroy()
        {
            FollowManager.DeregisterWeightProvider(this);
        }
    }
}