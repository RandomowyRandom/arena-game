using System;
using System.Data;
using EntitySystem.Abstraction;
using InteractionSystem;
using InteractionSystem.Abstraction;
using UnityEngine;

namespace EntitySystem
{
    [RequireComponent(typeof(Entity))]
    public class EntityKillInteraction: MonoBehaviour, IInteractable, IDamageSource
    {
        [SerializeField]
        private OutlineInteractionEffect _outlineInteractionEffect;
        
        [SerializeField]
        private InteractionTextHandler _interactionTextHandler;
        
        private Entity _entity;

        public GameObject GameObject => gameObject;

        private void Awake()
        {
            _entity = GetComponent<Entity>();
        }

        public void Interact(IInteractionHandler handler)
        {
            _entity.InstantKill(this);
        }

        public void OnHandlerEnter(IInteractionHandler handler)
        {
            _outlineInteractionEffect?.Show();
            _interactionTextHandler?.Show();
        }

        public void OnHandlerExit(IInteractionHandler handler)
        {
            _outlineInteractionEffect?.Hide();
            _interactionTextHandler?.Hide();
        }

        public GameObject Source
        {
            get => gameObject;
            set => throw new ReadOnlyException();
        }

        public EntityAttackerGroup AttackerGroup
        {
            get => EntityAttackerGroup.Other;
            set => throw new ReadOnlyException();
        }

        public float Damage => 0;
        public bool CanAttackEntity(Entity entity) => false;

        public void EntityAttacked(Entity entity) { }
    }
}