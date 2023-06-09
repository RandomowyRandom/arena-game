﻿using System;
using DG.Tweening;
using InteractionSystem;
using InteractionSystem.Abstraction;
using Player;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace WaveSystem
{
    public class FireplaceController: SerializedMonoBehaviour, IInteractable
    {
        [SerializeField] 
        private SpriteRenderer _light;
        
        [SerializeField]
        private Light2D _light2D;
        
        [SerializeField]
        private SpriteRenderer _fireplaceRenderer;
        
        [SerializeField]
        private SpriteRenderer _arenaRenderer;
        
        [FormerlySerializedAs("_glowLight2D")] [SerializeField]
        private Light2D _globalLight2D;
        
        [OdinSerialize]
        private IWaveManager _waveManager;
        
        [SerializeField]
        private InteractionTextHandler _interactionTextHandler;
        
        [SerializeField]
        private OutlineInteractionEffect _outlineInteractionEffect;
        
        public GameObject GameObject => gameObject;
        public void Interact()
        {
            if(_waveManager.IsWaveInProgress)
                return;
            
            _waveManager.StartWave();
            OnHandlerExit(null);
        }

        public void OnHandlerEnter(IInteractionHandler handler)
        {
            if(_waveManager.IsWaveInProgress)
                return;
            
            _fireplaceRenderer.material.SetFloat(_outlinePixelWidth, 1f);
            _interactionTextHandler?.Show();
            _outlineInteractionEffect?.Show();
        }

        public void OnHandlerExit(IInteractionHandler handler)
        {
            _fireplaceRenderer.material.SetFloat(_outlinePixelWidth, 0f);
            _interactionTextHandler?.Hide();
            _outlineInteractionEffect?.Hide();
        }

        private bool _state;
        private readonly int _outlinePixelWidth = Shader.PropertyToID("_OutlinePixelWidth");

        private void Awake()
        {
            _waveManager.OnWaveStart += Enable;
            _waveManager.OnWaveEnd += Disable;
        }

        private void Start()
        {
            SetState(false);
        }

        private void SetState(bool state)
        {
            _state = state;
            _light.gameObject.SetActive(state);
            _light2D.gameObject.SetActive(state);
        }

        private void Disable(Wave wave)
        {
            SetState(false);
            DOTween.To(
                () => _globalLight2D.intensity, 
                x => _globalLight2D.intensity = x, .9f, 1f);
            
            _arenaRenderer.gameObject.SetActive(false);
            _arenaRenderer.material.DOColor(Color.clear, 1f);
        }

        private void Enable(Wave wave)
        {
            SetState(true);
            DOTween.To(
                () => _globalLight2D.intensity, 
                x => _globalLight2D.intensity = x, .4f, 1f);
            
            _arenaRenderer.gameObject.SetActive(true);
            _arenaRenderer.material.DOColor(Color.white, 1f);
        }
    }
}