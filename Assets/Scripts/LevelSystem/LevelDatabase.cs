using System.Collections.Generic;
using System.Linq;
using Common.Attributes;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace LevelSystem
{
    [ScriptableFactoryElement]
    public class LevelDatabase: SerializedScriptableObject
    {
        [OdinSerialize]
        private List<Level> _levels;
        
        public Level GetLevel(int levelNumber)
        {
            return _levels.FirstOrDefault(level => level.LevelNumber == levelNumber);
        }
        
        [SerializeField]
        private int _levelAmount;
        
        [SerializeField]
        private float _experienceMultiplier = 1f;
        
        [Button]
        private void GenerateLevels()
        {
            _levels = new List<Level>();
            for (var i = 1; i <= _levelAmount; i++)
            {
                _levels.Add(new Level
                {
                    LevelNumber = i,
                    ExperienceRequired = Mathf.RoundToInt(i * 100 * _experienceMultiplier)
                });
            }
        }
    }
}