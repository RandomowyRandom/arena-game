﻿using DG.Tweening;
using EntitySystem.Abstraction;
using UnityEngine;

namespace EntitySystem
{
    [RequireComponent(typeof(Entity))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class EntityHitEffect: MonoBehaviour
    {
        [SerializeField]
        private AudioClip _hitSound;
        
        [SerializeField]
        private AudioClip _levelLockedSound;
        
        private Entity _entity;
        private SpriteRenderer _spriteRenderer;
        
        private readonly int _hitEffect = Shader.PropertyToID("_HitEffectBlend");
        private readonly int _hitEffectColor = Shader.PropertyToID("_HitEffectColor");

        private void Awake()
        {
            _entity = GetComponent<Entity>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            _entity.OnDamageTaken += UpdateMaterial;
            _entity.OnDamageTaken += ScaleEnemy;
        }

        private void ScaleEnemy(float damage, IDamageSource source)
        {
            transform.DOKill();
             
            if(damage <= 0)
                return;
            
            transform.localScale = Vector2.one * new Vector2(1, 1.3f);
            transform.DOScale(1f, 0.1f);
        }

        private void OnDestroy()
        {
            _entity.OnDamageTaken -= UpdateMaterial;
            _spriteRenderer.DOKill();
        }

        private void UpdateMaterial(float damage, IDamageSource source)
        {
            _spriteRenderer.material.DOKill();

            var damageable = damage > 0;
            
            var soundEffect = damageable ? _hitSound : _levelLockedSound;
            
            _spriteRenderer.material.SetColor(_hitEffectColor, damageable ? Color.white : Color.red);
            _spriteRenderer.material.SetFloat(_hitEffect, 1);
            
            AudioSource.PlayClipAtPoint(soundEffect, transform.position);
            
            if(_spriteRenderer == null)
                return;
            
            _spriteRenderer.material.DOFloat(0, _hitEffect,  damageable ? .4f : 1f);
        }
    }
}