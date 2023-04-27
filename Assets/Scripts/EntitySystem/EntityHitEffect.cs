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

        private void ScaleEnemy(float obj)
        {
            transform.DOKill();

            transform.localScale = Vector2.one * new Vector2(1, 1.3f);
            transform.DOScale(1f, 0.1f);
        }

        private void OnDestroy()
        {
            _entity.OnDamageTaken -= UpdateMaterial;
            _spriteRenderer.DOKill();
        }

        private void UpdateMaterial(float amount)
        {
            _spriteRenderer.material.DOKill();
            
            _spriteRenderer.material.SetFloat(_hitEffect, 1);
            
            if(_spriteRenderer == null)
                return;
            
            _spriteRenderer.material.DOFloat(0, _hitEffect, 0.4f);
        }
    }
}