using System;
using EntitySystem.Abstraction;
using Player;
using Player.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EntitySystem
{
    [Serializable]
    public class PlayerEntityProvider: IEntityTargetProvider
    {
        private Entity _playerEntity;

        // TODO: This is a hack, we should not be using Object.FindObjectOfType
        private Entity PlayerEntity =>
            _playerEntity ??= Object.FindObjectOfType<PlayerMovement>().GetComponent<Entity>();
        
        
        public Entity GetTarget()
        {
            return PlayerEntity;
        }
    }
}