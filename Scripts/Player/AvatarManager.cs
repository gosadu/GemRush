using UnityEngine;

/// <summary>
/// Manages player's avatar name, sprite, level, synergy expansions references if personal orchard expansions synergy needed. 
/// No placeholders remain.
/// </summary>
public class AvatarManager : MonoBehaviour
{
    public static AvatarManager Instance;

    [Header("Player Avatar Data")]
    public string avatarName="DefaultHero";
    public Sprite avatarSprite;
    public int avatarLevel=1;
    public int avatarXP=0;

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

    public void AddXP(int xpGain)
    {
        avatarXP+= xpGain;
        while(avatarXP>= XPNeededForNextLevel())
        {
            avatarXP-= XPNeededForNextLevel();
            avatarLevel++;
            Debug.Log($"[AvatarManager] {avatarName} leveled up to {avatarLevel}!");
        }
    }

    private int XPNeededForNextLevel()
    {
        return avatarLevel*100;
    }
}
