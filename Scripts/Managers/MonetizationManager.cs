using UnityEngine;

/// <summary>
/// Minimal logs: only warnings for real integration. Currently placeholders for ads/IAP.
/// </summary>
public class MonetizationManager : MonoBehaviour
{
    public void ShowInterstitialAd()
    {
        // No logs to reduce spam
    }

    public void ShowRewardedAd(System.Action onReward)
    {
        // ...
    }

    public void PurchaseItem(string productId)
    {
        // ...
    }
}
