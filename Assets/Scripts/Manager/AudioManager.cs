using UnityEngine;

namespace Manager
{
    public class AudioManager : MonoBehaviour 
    {
        public AudioSource audioSource;
        
        [Header("Sound")] 
        public AudioClip loadingSound;
        public AudioClip mainSound;
        public AudioClip buttonSound;

        public void PlaySound(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        public void StopSound(AudioClip audioClip)
        {
            audioSource.Stop();
        }

        public void ChangeVolume(float volume)
        {
            audioSource.volume = volume;
        }

        public void PlayButtonSound()
        {
            audioSource.PlayOneShot(buttonSound, 0.15f);
        }
    }
}
