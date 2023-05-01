using System;
using JetBrains.Annotations;
using LevelSystem.Abstraction;
using Player.Interfaces;
using QFSW.QC;
using UnityEngine;

namespace LevelSystem
{
    public class PlayerLevel: MonoBehaviour, IPlayerLevel
    {
        [SerializeField]
        private LevelDatabase _levelDatabase;
        
        [SerializeField]
        private ParticleSystem _gainExperienceEffect;
        
        public event Action<int> OnLevelUp;
        
        private int _currentLevel = 1;
        private int _currentExperience;
        
        public int CurrentLevel => _currentLevel;
        public int CurrentExperience => _currentExperience;

        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IPlayerLevel>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerLevel>();
        }

        public void AddExperience(int experience)
        {
            _currentExperience += experience;
            if (_currentExperience < _levelDatabase.GetLevel(_currentLevel).ExperienceRequired) 
                return;
            
            _currentLevel++;
            _currentExperience = 0;
            
            OnLevelUp?.Invoke(_currentLevel);
        }
        
        public void OnExperienceProviderEnter(GameObject collision)
        {
            var experienceProvider = collision.GetComponent<IExperienceProvider>();
            if (experienceProvider == null) 
                return;
            
            AddExperience(experienceProvider.GetExperience());
            Instantiate(_gainExperienceEffect, collision.transform.position, Quaternion.identity);
            Destroy(collision);
        }

        #region QC

        [Command("log-level")] [UsedImplicitly]
        private void CommandLogLevel()
        {
            Debug.Log($"Current level: {_currentLevel}");
            Debug.Log($"Experience: {_currentExperience} / {_levelDatabase.GetLevel(_currentLevel).ExperienceRequired}");
        }

        #endregion
    }
}