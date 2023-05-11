using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using LevelSystem.Abstraction;
using Notifications.Abstraction;
using Player.Interfaces;
using QFSW.QC;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;

namespace LevelSystem
{
    public class PlayerLevel: MonoBehaviour, IPlayerLevel
    {
        [SerializeField]
        private LevelDatabase _levelDatabase;
        
        [SerializeField]
        private ParticleSystem _gainExperienceEffect;
        
        [SerializeField]
        private GameObject _levelUpEffect;
        
        [SerializeField]
        private AudioClip _levelUpSound;

        [SerializeField] 
        private CinemachineVirtualCamera _virtualCamera;
        
        public event Action OnLevelChanged;
        
        public event Action<Level> OnLevelUp;
        
        private int _currentLevel = 1;
        private int _currentExperience;
        
        private IPlayerNotificationHandler _notificationHandler;
        
        public int CurrentLevel => _currentLevel;
        public int CurrentExperience => _currentExperience;
        public int NextLevelExperience => _levelDatabase.GetLevel(_currentLevel).ExperienceRequired;

        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IPlayerLevel>(this);
        }

        private void Start()
        {
            _notificationHandler = ServiceLocator.ServiceLocator.Instance.Get<IPlayerNotificationHandler>();
            
            OnLevelUp += SpawnParticleEffect;
            OnLevelUp += PlaySound;
            OnLevelUp += PanCamera;
            OnLevelUp += InstantiateNotification;
        }

        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerLevel>();
            
            OnLevelUp -= SpawnParticleEffect;
            OnLevelUp -= PlaySound;
            OnLevelUp -= PanCamera;
        }

        public void AddExperience(int experience)
        {
            _currentExperience += experience;
            
            OnLevelChanged?.Invoke();
            
            if (_currentExperience < _levelDatabase.GetLevel(_currentLevel).ExperienceRequired) 
                return;
            
            _currentLevel++;
            _currentExperience = 0;
            
            OnLevelUp?.Invoke(_levelDatabase.GetLevel(_currentLevel));
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
        
        private void PanCamera(Level level)
        {
            _virtualCamera.m_Lens.OrthographicSize = 7.3f;
            
            DOTween.To(() => _virtualCamera.m_Lens.OrthographicSize, x => _virtualCamera.m_Lens.OrthographicSize = x, 6.3f, 1f)
                .SetEase(Ease.OutCubic);
        }
        
        private void SpawnParticleEffect(Level level)
        {
            Instantiate(_levelUpEffect, transform.position, Quaternion.identity);
        }
        
        private void PlaySound(Level level)
        {
            AudioSource.PlayClipAtPoint(_levelUpSound, transform.position);
        }
        
        private void InstantiateNotification(Level level)
        {
            if(level.LevelInfo.IsNullOrWhitespace())
                return;
            
            _notificationHandler.ForceSendNotification(level.LevelInfo, 3f);
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