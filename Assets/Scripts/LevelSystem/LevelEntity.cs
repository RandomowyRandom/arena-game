using System;
using LevelSystem.Abstraction;
using UnityEngine;

namespace LevelSystem
{
    public class LevelEntity: MonoBehaviour
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
    }
}