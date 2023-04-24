using UnityEngine;

namespace TriangularAssets
{
    public class AutoDestroyer : MonoBehaviour
    {
        [SerializeField] float _timeToDestroy = 1f;
        
        private void Start()
        {
            Destroy(gameObject, _timeToDestroy);
        }
    }
}