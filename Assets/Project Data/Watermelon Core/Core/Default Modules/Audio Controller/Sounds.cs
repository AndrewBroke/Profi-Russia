using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public class Sounds
    {
        [Header("UI")]
        public AudioClip assembleSound;
        public AudioClip loseSound;
        public AudioClip coinSound;
        public AudioClip buttonSound;
        public AudioClip winSound;
        public AudioClip powerupSound;
    }
}