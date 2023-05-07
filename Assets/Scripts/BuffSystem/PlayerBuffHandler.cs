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
        public event Action OnBuffChanged;
        
        public GameObject GameObject => gameObject;
        
        private readonly List<Buff> _buffs = new();

        private StatsData _statsData = new();

        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IBuffHandler>(this);
        }
        
        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IBuffHandler>();
        }

        private void Update()
        {
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
                OnBuffChanged?.Invoke();
                return;
            }
            
            _buffs.Add(new Buff(buffData));
            var effects = buffData.Effects;
            
            foreach (var effect in effects)
            {
                effect.OnApply(this);
            }
            
            OnBuffChanged?.Invoke();
        }

        public void RemoveBuff(Buff buff)
        {
            _buffs.Remove(buff);
            var effects = buff.BuffData.Effects;
            
            foreach (var effect in effects)
            {
                effect.OnRemove(this);
            }
            
            OnBuffChanged?.Invoke();
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