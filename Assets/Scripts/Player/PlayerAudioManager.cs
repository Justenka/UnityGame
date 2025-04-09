using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Effect Clips")]
    public AudioClip strangeEffectClip;

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
            audioSource.PlayOneShot(clip);
    }

    public void PlayStrangeEffect() => PlaySound(strangeEffectClip);
}
