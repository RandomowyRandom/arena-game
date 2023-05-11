using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(PlayerDashHandler))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerDashEffect: MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _dashParticles;
        
        [SerializeField]
        private CinemachineVirtualCamera _virtualCamera;
        
        [SerializeField]
        private AudioClip _dashSound;
        
        private PlayerDashHandler _playerDashHandler;
        private SpriteRenderer _spriteRenderer;
        
        private readonly int _hitEffectBlend = Shader.PropertyToID("_HitEffectBlend");
        private readonly int _hitEffectColor = Shader.PropertyToID("_HitEffectColor");

        private void Awake()
        {
            _playerDashHandler = GetComponent<PlayerDashHandler>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        private void Start()
        {
            _playerDashHandler.OnDash += PerformDashEffect;
        }

        private void OnDestroy()
        {
            _playerDashHandler.OnDash -= PerformDashEffect;
        }

        private async void PerformDashEffect()
        {
            _spriteRenderer.material.DOKill();
            transform.DOKill();
            
            _spriteRenderer.material.SetFloat(_hitEffectBlend, 1f);
            _spriteRenderer.material.SetColor(_hitEffectColor, Color.yellow);
            _virtualCamera.m_Lens.OrthographicSize = 6.9f;
            _dashParticles.Play();
            AudioSource.PlayClipAtPoint(_dashSound, transform.position);
            transform.localScale = new Vector3(1.3f, .8f, 1);
            
            await UniTask.Delay(TimeSpan.FromSeconds(.1f));
            DOTween.To(
                () => _virtualCamera.m_Lens.OrthographicSize, 
                x => _virtualCamera.m_Lens.OrthographicSize = x, 
                6.3f, .25f);
            transform.DOScale(Vector3.one, .2f);
            _spriteRenderer.material.DOFloat(0, _hitEffectBlend, .2f);
            await UniTask.Delay(TimeSpan.FromSeconds(.1f));
            _dashParticles.Stop();
        }
    }
}