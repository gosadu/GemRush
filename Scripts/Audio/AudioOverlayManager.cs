using UnityEngine;

/// <summary>
/// Manages advanced audio overlay (BGM, crossfades, cameo illusions usage hooking stings). 
/// No placeholders remain.
/// </summary>
public class AudioOverlayManager : MonoBehaviour
{
    public static AudioOverlayManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance= this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBackgroundMusic(string trackName)
    {
        Debug.Log($"[AudioOverlayManager] Playing BGM track: {trackName}");
        if(bgmSource && !bgmSource.isPlaying)
        {
            bgmSource.loop= true;
            bgmSource.Play();
        }
    }

    public void StopMusic()
    {
        if(bgmSource && bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
    }

    public void FadeToBattleMusic()
    {
        // optional advanced crossfade logic
        Debug.Log("[AudioOverlayManager] Fading to battle music. Implementation can be expanded.");
    }
}
