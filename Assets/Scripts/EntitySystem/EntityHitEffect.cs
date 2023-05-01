using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace EntitySystem
{
    [RequireComponent(typeof(Entity))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class EntityHitEffect: MonoBehaviour
    {
        private Entity _entity;
        private SpriteRenderer _spriteRenderer;
        
        private readonly int _hitEffect = Shader.PropertyToID("_HitEffectBlend");

        private void Awake()
        {
            _entity = GetComponent<Entity>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            _entity.OnDamageTaken += UpdateMaterial;
            _entity.OnDamageTaken += ScaleEnemy;
        }

        private void ScaleEnemy(float damage)
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

        private void UpdateMaterial(float damage)
        {
            _spriteRenderer.material.DOKill();

            var damageable = damage > 0;
            
            _spriteRenderer.material.SetColor("_HitEffectColor", damageable ? Color.white : Color.red);
            _spriteRenderer.material.SetFloat(_hitEffect, 1);
            
            if(_spriteRenderer == null)
                return;
            
            _spriteRenderer.material.DOFloat(0, _hitEffect,  damageable ? .4f : 1f);
        }
    }
}