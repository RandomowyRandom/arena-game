using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace TriangularAssets
{
    public class AutoDestroyer : MonoBehaviour
    {
        [SerializeField]
        float _timeToDestroy = 1f;

        [SerializeField]
        private bool _useDoTween = false;
        
        private async void Start()
        {
            if (!_useDoTween)
                Destroy(gameObject, _timeToDestroy);
            else
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_timeToDestroy));
                transform.DOScale(Vector3.zero, .3f).OnComplete(() => Destroy(gameObject));
            }
        }
    }
}