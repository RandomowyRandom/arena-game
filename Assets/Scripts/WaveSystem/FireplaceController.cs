using System;
using DG.Tweening;
using Player;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace WaveSystem
{
    public class FireplaceController: SerializedMonoBehaviour
    {
        [SerializeField] 
        private SpriteRenderer _light;
        
        [SerializeField]
        private Light2D _light2D;
        
        [SerializeField]
        private SpriteRenderer _fireplaceRenderer;
        
        [FormerlySerializedAs("_glowLight2D")] [SerializeField]
        private Light2D _globalLight2D;
        
        [OdinSerialize]
        private IWaveManager _waveManager;
        
        private bool _state;
        private bool _isPlayerNearby;
        private readonly int _outlinePixelWidth = Shader.PropertyToID("_OutlinePixelWidth");
        private readonly int _innerOutlineThickness = Shader.PropertyToID("_InnerOutlineThickness");

        private void Awake()
        {
            _waveManager.OnWaveStart += Enable;
            _waveManager.OnWaveEnd += Disable;
        }

        private void Start()
        {
            SetState(false);
        }

        private void Update()
        {
            if(!_isPlayerNearby)
                return;

            if(_waveManager.IsWaveInProgress)
                return;
            
            if (Keyboard.current.eKey.wasPressedThisFrame)
                _waveManager.StartWave();
        }

        private void SetState(bool state)
        {
            _state = state;
            _light.gameObject.SetActive(state);
            _light2D.gameObject.SetActive(state);
            _fireplaceRenderer.material.SetFloat(_innerOutlineThickness, state ? 1f : 0f);
        }

        public void OnPlayerEnter(GameObject collision)
        {
            var player = collision.GetComponent<PlayerMovement>();
            if (player == null) 
                return;
            
            if(_waveManager.IsWaveInProgress)
                return;
            
            _fireplaceRenderer.material.SetFloat(_outlinePixelWidth, 1f);
            _isPlayerNearby = true;
        }
        
        public void OnPlayerExit(GameObject collision)
        {
            var player = collision.GetComponent<PlayerMovement>();
            if (player == null) 
                return;
            
            _fireplaceRenderer.material.SetFloat(_outlinePixelWidth, 0f);
            _isPlayerNearby = false;
        }
        
        private void Disable(Wave wave)
        {
            SetState(false);
            DOTween.To(
                () => _globalLight2D.intensity, 
                x => _globalLight2D.intensity = x, .9f, 1f);
        }

        private void Enable(Wave wave)
        {
            SetState(true);
            DOTween.To(
                () => _globalLight2D.intensity, 
                x => _globalLight2D.intensity = x, .4f, 1f);
        }
    }
}