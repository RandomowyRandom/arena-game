using BuffSystem.Abstraction;
using TriangularAssets;
using UnityEngine;

namespace BuffSystem
{
    [RequireComponent(typeof(CollisionHandler))]
    public class CollisionBuff: MonoBehaviour
    {
        [SerializeField]
        private BuffData _buffData;
        
        public void OnBuffHandlerEnter(GameObject collision)
        {
            var buffHandler = collision.GetComponent<IBuffHandler>();

            buffHandler?.AddBuff(_buffData);
        }
    }
}