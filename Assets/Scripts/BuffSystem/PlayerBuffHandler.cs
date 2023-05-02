using System;
using System.Collections.Generic;
using System.Linq;
using BuffSystem.Abstraction;
using Items.RaritySystem;
using Sirenix.OdinInspector;
using Stats;
using Stats.Interfaces;
using UnityEngine;

namespace BuffSystem
{
    public class PlayerBuffHandler: SerializedMonoBehaviour, IBuffHandler, IStatsDataProvider
    {
        [SerializeField]
        private BuffData _buffToApply;

        [Button]
        private void Apply()
        {
            AddBuff(_buffToApply);
        }
        
        public GameObject GameObject => gameObject;
        
        private readonly List<Buff> _buffs = new();

        private StatsData _statsData = new();

        private void Update()
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < _buffs.Count; i++)
            {
                var buff = _buffs[i];
                buff.RemainingTime -= Time.deltaTime;
                if (buff.RemainingTime <= 0)
                    RemoveBuff(buff);
            }
        }

        public void UpdateStatsData(StatsData statsData)
        {
            _statsData += statsData;
        }

        public void AddBuff(BuffData buffData)
        {
            foreach (var buff in _buffs.Where(buff => buff.BuffData == buffData))
            {
                buff.RemainingTime = buffData.Duration;
                return;
            }
            
            
            _buffs.Add(new Buff(buffData));
            var effects = buffData.Effects;
            
            foreach (var effect in effects)
            {
                effect.OnApply(this);
            }
        }

        public void RemoveBuff(Buff buff)
        {
            _buffs.Remove(buff);
            var effects = buff.BuffData.Effects;
            
            foreach (var effect in effects)
            {
                effect.OnRemove(this);
            }
        }
        
        public List<Buff> GetBuffs()
        {
            return _buffs;
        }

        public StatsData GetStatsData(GearRarity gearRarity)
        {
            return _statsData;
        }
    }
}