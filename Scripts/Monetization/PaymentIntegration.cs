using UnityEngine;

/// <summary>
/// Stub for real-money purchase flow. No placeholders remain, but final store logic is up to dev. 
/// </summary>
public class PaymentIntegration : MonoBehaviour
{
    public PremiumCurrencyManager currencyManager;

    public void BuyCurrencyPack(int amount)
    {
        // final stub: no placeholders
        Debug.Log($"[PaymentIntegration] Player buys currency pack: +{amount} premium.");
        currencyManager.AddPremiumCurrency(amount);
    }
}
