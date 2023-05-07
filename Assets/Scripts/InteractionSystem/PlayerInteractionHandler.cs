using System;
using System.Collections.Generic;
using System.Linq;
using InteractionSystem.Abstraction;
using TriangularAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionSystem
{
    [RequireComponent(typeof(CollisionHandler))]
    public class PlayerInteractionHandler: MonoBehaviour, IInteractionHandler
    {
        private readonly List<IInteractable> _intractables = new();
        
        public GameObject GameObject => gameObject;

        private IInteractable _currentInteractable;
        
        private void FixedUpdate()
        {
            if(_intractables.Count == 0)
                return;
            
            _currentInteractable = GetNearestInteractable();

            foreach (var interactable in _intractables.Where(interactable => interactable != _currentInteractable))
                interactable.OnHandlerExit(this);

            _currentInteractable.OnHandlerEnter(this);
        }

        private void Update()
        {
            if(!Keyboard.current.eKey.wasPressedThisFrame)
                return;
            
            _currentInteractable?.Interact();
        }

        public void OnInteractableEnter(GameObject collision)
        {
            var interactable = collision.GetComponent<IInteractable>();
            
            if (interactable == null)
                return;
            
            _intractables.Add(interactable);
            
            interactable.OnHandlerEnter(this);
        }
        
        public void OnInteractableExit(GameObject collision)
        {
            var interactable = collision.GetComponent<IInteractable>();
            
            if (interactable == null)
                return;
            
            _intractables.Remove(interactable);
            
            interactable.OnHandlerExit(this);
            
            if(_currentInteractable == interactable)
                _currentInteractable = null;
        }

        private IInteractable GetNearestInteractable()
        {
            return _intractables.OrderBy
                (x => Vector3.Distance(
                    x.GameObject.transform.position,
                    transform.position))
                .FirstOrDefault();
        }
    }
}