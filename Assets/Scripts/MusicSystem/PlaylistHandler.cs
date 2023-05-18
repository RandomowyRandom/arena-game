﻿using System;
using Common.Extensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using WaveSystem;

namespace MusicSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class PlaylistHandler: MonoBehaviour
    {
        [SerializeField]
        private Playlist _playlist;

        private AudioSource _audioSource;
        
        private IWaveManager _waveManager;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _waveManager = ServiceLocator.ServiceLocator.Instance.Get<IWaveManager>();
            ServiceLocator.ServiceLocator.Instance.OnServiceRegistered += OnServiceRegistered;
            
            PlayTrack(_playlist.OutOfCombatClips.GetRandomElement(), 0, .5f);
            
            if(_waveManager == null)
                return;
            
            _waveManager.OnWaveStart += PlayCombatTrack;
            _waveManager.OnWaveEnd += PlayOutOfCombatTrack;
        }

        private void OnServiceRegistered(Type type)
        {
            if (type != typeof(IWaveManager)) 
                return;

            if (_waveManager != null)
            {
                _waveManager.OnWaveStart -= PlayCombatTrack;
                _waveManager.OnWaveEnd -= PlayOutOfCombatTrack;
            }
            
            _waveManager = ServiceLocator.ServiceLocator.Instance.Get<IWaveManager>();
            
            _waveManager.OnWaveStart += PlayCombatTrack;
            _waveManager.OnWaveEnd += PlayOutOfCombatTrack;
        }

        private async void PlayOutOfCombatTrack(Wave wave)
        {
            var transitionTrack = _playlist.CombatEndClip;
            var track = _playlist.OutOfCombatClips.GetRandomElement();
            await PlayTrackUntilFinished(transitionTrack, 0f, 1f);
            
            PlayTrack(track, 1f, 1f);
        }

        private void PlayCombatTrack(Wave wave)
        {
            var track = _playlist.CombatClip;
            PlayTrack(track, 1f, 1f);
        }

        private void PlayTrack(AudioClip clip, float fromTime = 0f, float toTime = 0f)
        {
            DOTween.To(() => _audioSource.volume, x => _audioSource.volume = x, 0f, fromTime).OnComplete(() =>
            {
                _audioSource.clip = clip;
                _audioSource.Play();
                DOTween.To(() => _audioSource.volume, x => _audioSource.volume = x, 1f, toTime);
            });
        }

        private async UniTask PlayTrackUntilFinished(AudioClip clip, float fromTime = 0f, float toTime = 0f)
        {
            PlayTrack(clip, fromTime, toTime);
            await UniTask.Delay(TimeSpan.FromSeconds(clip.length));
        }
        
    }
}