using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerWalkSoundHandler: MonoBehaviour
    {
        [SerializeField]
        private List<AudioClip> _walkSounds;
        
        public void PlayRandomWalkSound()
        {
            var randomIndex = Random.Range(0, _walkSounds.Count);
            var randomWalkSound = _walkSounds[randomIndex];
            
            AudioSource.PlayClipAtPoint(randomWalkSound, transform.position, .4f);
        }
    }
}