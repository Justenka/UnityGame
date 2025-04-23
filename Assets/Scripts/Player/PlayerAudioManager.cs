using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Effect Clips")]
    public AudioClip strangeEffectClip;
    public AudioClip walkingAudioSource;
    public AudioClip runningAudioSource;

    void Awake()
    {
        // Ensure there is an AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    //Plays an audio clip once.
    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            if(clip == strangeEffectClip)
            {
                audioSource.volume = 1f;
                audioSource.PlayOneShot(clip);
                audioSource.volume = 0.3f;
            }
            audioSource.PlayOneShot(clip);
        }
    }
    public void PlayLoop(AudioClip clip)
    {
        if (audioSource.clip != clip || !audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopLoop()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
        audioSource.loop = false;
    }

    public void PlayStrangeEffect() => PlaySound(strangeEffectClip);
    public void PlayWalkingAudio() => PlaySound(walkingAudioSource);
    public void PlayRunningAudio() => PlaySound(runningAudioSource);
}
