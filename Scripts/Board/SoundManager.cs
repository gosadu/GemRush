using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioSource musicSource;
    public AudioClip matchClip;
    public AudioClip swapClip;

    void Start()
    {
        if (musicSource) musicSource.Play();
    }

    public void PlayMatchSound()
    {
        if (sfxSource && matchClip) sfxSource.PlayOneShot(matchClip);
    }

    public void PlaySwapSound()
    {
        if (sfxSource && swapClip) sfxSource.PlayOneShot(swapClip);
    }
}
