using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource combatSource;

    [Header("Effect Clips")]
    public AudioClip strangeEffectClip;
    public AudioClip walkingAudioSource;
    public AudioClip runningAudioSource;
    public AudioClip swoosh;
    public AudioClip pop;
    public AudioClip gearing;
    public AudioClip drink;
    public AudioClip money;
    public AudioClip interact;



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
        if (combatSource != null && clip != null)
        {
            combatSource.PlayOneShot(clip);
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
}
