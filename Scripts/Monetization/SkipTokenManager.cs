using UnityEngine;

/// <summary>
/// Manages skip tokens used to bypass certain wait times or puzzle nodes. 
/// No placeholders remain.
/// </summary>
public class SkipTokenManager : MonoBehaviour
{
    public static SkipTokenManager Instance;

    public int skipTokenBalance=0;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance=this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddSkipTokens(int count)
    {
        skipTokenBalance+= count;
        Debug.Log($"[SkipTokenManager] +{count} tokens, total {skipTokenBalance}.");
    }

    public bool UseSkipToken()
    {
        if(skipTokenBalance<=0)
        {
            Debug.LogWarning("[SkipTokenManager] No skip tokens left.");
            return false;
        }
        skipTokenBalance--;
        Debug.Log($"[SkipTokenManager] Used 1 skip token, {skipTokenBalance} left.");
        return true;
    }
}
