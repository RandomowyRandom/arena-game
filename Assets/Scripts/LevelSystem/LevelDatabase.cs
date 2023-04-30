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
        [OdinSerialize] [ReadOnly]
        private List<Level> _levels;
        
        public Level GetLevel(int levelNumber)
        {
            return _levels.FirstOrDefault(level => level.LevelNumber == levelNumber);
        }
        
        [SerializeField]
        private int _levelAmount;
        
        
        [Button]
        private void GenerateLevels()
        {
            _levels = new List<Level>();
            for (var i = 1; i <= _levelAmount; i++)
            {
                _levels.Add(new Level
                {
                    LevelNumber = i,
                    ExperienceRequired = i * 100
                });
            }
        }
    }
}