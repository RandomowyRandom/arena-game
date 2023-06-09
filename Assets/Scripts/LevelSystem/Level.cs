﻿using System;
using UnityEngine;

namespace LevelSystem
{
    [Serializable]
    public class Level
    {
        [SerializeField]
        private int _levelNumber;
        
        [SerializeField]
        private int _experienceRequired;
        
        [SerializeField]
        private GameObject _levelRewardPrefab;

        [SerializeField] 
        private string _levelInfo;

        public int LevelNumber
        {
            get => _levelNumber;
            set => _levelNumber = value;
        }

        public int ExperienceRequired
        {
            get => _experienceRequired;
            set => _experienceRequired = value;
        }
        
        public GameObject LevelRewardPrefab
        {
            get => _levelRewardPrefab;
            set => _levelRewardPrefab = value;
        }
        
        public string LevelInfo
        {
            get => _levelInfo;
            set => _levelInfo = value;
        }
    }
}