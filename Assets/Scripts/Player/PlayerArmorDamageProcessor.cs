using System;
using EntitySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Stats.Interfaces;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerArmorDamageProcessor: SerializedMonoBehaviour, IDamageProcessor
    {
        private PlayerStats _playerStats;

        private void Awake()
        {
            _playerStats = GetComponent<PlayerStats>();
        }

        public float Process(float damage)
        {
            var armor = _playerStats.GetStatsData();

            var processedDamage = damage;
            
            processedDamage -= armor.Defense * .5f;
            processedDamage = Mathf.Clamp(processedDamage, 1, damage);

            return processedDamage;
        }
    }
}