using ServiceLocator;
using UnityEngine;

namespace EntitySystem.ObjectAwareFollow.Abstraction
{
    public interface IObjectAwareFollowManager: IService
    {
        public void RegisterWeightProvider(FollowWeightProvider provider);
        public void DeregisterWeightProvider(FollowWeightProvider provider);
        public Vector2 GetVectorForTransform(Transform providedTransform, float distanceTolerance = 2f);
    }
}