using System;
using Cysharp.Threading.Tasks;
using LevelSystem.Abstraction;
using Player;
using Player.Interfaces;
using UnityEngine;

namespace LevelSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ExperienceOrb: MonoBehaviour, IExperienceProvider
    {
        private Transform _playerTransform;
        private Rigidbody2D _rigidbody2D;

        private bool _isActivated = false;
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private async void Start()
        {
            _playerTransform = FindObjectOfType<PlayerStamina>().transform;
            
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            _isActivated = true;
        }

        private void FixedUpdate()
        {
            if(!_isActivated)
                return;
            
            var direction = (_playerTransform.position - transform.position).normalized;
            _rigidbody2D.AddForce(direction * 90f);
        }

        public int GetExperience()
        {
            return 10;
        }
    }
}