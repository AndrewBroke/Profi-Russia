using UnityEngine;

namespace Watermelon
{
    [SetupTab("Audio", texture = "icon_audio")]
    [CreateAssetMenu(fileName = "Audio Settings", menuName = "Settings/Audio Settings")]
    public class AudioSettings : ScriptableObject
    {
        [SerializeField] AudioClip[] musicAudioClips;
        public AudioClip[] MusicAudioClips => musicAudioClips;

        [SerializeField] Sounds sounds;
        public Sounds Sounds => sounds;

        [SerializeField] Vibrations vibrations;
        public Vibrations Vibrations => vibrations;
    }
}

// -----------------
// Audio Controller v 0.3.2
// -----------------