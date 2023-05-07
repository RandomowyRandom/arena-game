using System;
using Cysharp.Threading.Tasks;
using Items.Abstraction;
using Items.ItemDataSystem;
using UnityEngine;

namespace Items.Effects
{
    [Serializable]
    public class PlaySoundItemEffect: IItemEffect
    {
        [SerializeField]
        private AudioClip _clip;
        
        public UniTask OnUse(IItemUser user, UsableItem item)
        {
            AudioSource.PlayClipAtPoint(_clip, user.GameObject.transform.position);
            return UniTask.CompletedTask;
        }
    }
}