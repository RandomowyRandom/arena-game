﻿using UnityEngine;

namespace InteractionSystem.Abstraction
{
    public interface IInteractable
    {
        public GameObject GameObject { get; }
        
        public void Interact();
        
        public void OnHandlerEnter(IInteractionHandler handler);
        public void OnHandlerExit(IInteractionHandler handler);
    }
}