using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using EntitySystem;
using EntitySystem.Abstraction;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Entity))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerEntity: MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera _virtualCamera;
        
        [SerializeField]
        private AudioClip _hitSound;
        
        private Entity _entity;
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private readonly int _hitEffectBlend = Shader.PropertyToID("_HitEffectBlend");
        private readonly int _hitEffectColor = Shader.PropertyToID("_HitEffectColor");

        private void Awake()
        {
            _entity = GetComponent<Entity>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _entity.OnDamageTaken += ApplyKnockBack;
            _entity.OnDamageTaken += ShowHitEffect;
        }

        private async void ShowHitEffect(float damage, IDamageSource source)
        {
            _spriteRenderer.material.SetColor(_hitEffectColor, Color.red);
            _spriteRenderer.material.SetFloat(_hitEffectBlend, 1);
            _spriteRenderer.material.DOFloat(0, _hitEffectBlend,  .3f);
            
            AudioSource.PlayClipAtPoint(_hitSound, transform.position);
            Time.timeScale = 0f;
            _virtualCamera.m_Lens.OrthographicSize = 4.9f;
            await UniTask.Delay(TimeSpan.FromSeconds(.1f), DelayType.Realtime);
            DOTween.To(
                () => _virtualCamera.m_Lens.OrthographicSize, 
                x => _virtualCamera.m_Lens.OrthographicSize = x, 
                6.3f, .3f);
            
            Time.timeScale = 1f;
        }


        private void OnDestroy()
        {
            _entity.OnDamageTaken -= ApplyKnockBack;
            _entity.OnDamageTaken -= ShowHitEffect;
        }

        private void ApplyKnockBack(float damage, IDamageSource source)
        {
            var direction = (transform.position - source.Source.transform.position).normalized;
            _rigidbody2D.AddForce(direction * 10f, ForceMode2D.Impulse);
        }
    }
}