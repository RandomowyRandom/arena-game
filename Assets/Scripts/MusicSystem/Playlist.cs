using System.Collections.Generic;
using Common.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MusicSystem
{
    [ScriptableFactoryElement]
    public class Playlist: SerializedScriptableObject
    {
        [SerializeField]
        private List<AudioClip> _outOfCombatClips;
        
        [SerializeField]
        private AudioClip _combatClip;
        
        [SerializeField]
        private AudioClip _combatEndClip;
        
        [SerializeField]
        private AudioClip _bossClip;

        public List<AudioClip> OutOfCombatClips => _outOfCombatClips;

        public AudioClip CombatClip => _combatClip;

        public AudioClip CombatEndClip => _combatEndClip;

        public AudioClip BossClip => _bossClip;
    }
}