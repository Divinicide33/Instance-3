using UnityEngine;

    [CreateAssetMenu(fileName = "Nex SFX", menuName = "AudioManager/SFX")]
    public class SFXAsset : ScriptableObject
    {
        public string sfxName;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        public bool loop = false;
    }
